package main

import (
	"context"
	"encoding/json"
	"fmt"
	"net/http"
	"time"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/uow"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/worker"
	mylogger "github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
	usescases "github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/usecases"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/helplog"
	"github.com/jackc/pgx/v5/pgxpool"
)

var config Config
var pgpool *pgxpool.Pool
var rabbitMqUri string
var logger mylogger.ILog

func init() {
	logger = initLog()
	pgpool = initDb()
	rabbitMqUri = worker.UriBuilder(worker.RabbitMqConn{
		User:     config.MqUser,
		Password: config.MqPassword,
		Host:     config.MqHost,
		Port:     config.MqPort,
	},
	)
}

func main() {
	ctx := context.Background()
	defer pgpool.Close()

	if err := pgpool.Ping(ctx); err != nil {
		helplog.FatalIfError(err, "Cannot ping postgres")
	}

	w, err := worker.NewWorker(logger, worker.WorkerOpts{
		PoolSize:    1,
		RabbitMqUri: rabbitMqUri,
		QeueuName:   "readability-worker",
	})

	if err != nil {
		helplog.FatalIfError(err, "Cannot create worker")
	}

	forever := make(chan struct{})

	w.Run(work)

	<-forever
}

func work(data string, ctx context.Context) error {
	useCase := assembleUseCase()
	unitOfWork := uow.NewUnitOfWork(pgpool, logger)

	var evt domain.CreateReadabilityEvt

	err := json.Unmarshal([]byte(data), &evt)
	if err != nil {
		logger.Error("failed to unmarshal message",
			"data", data,
			"error", err,
		)
		return fmt.Errorf("unmarshal event: %w", domain.NewInvalidMessageError(err))
	}

	logger.Info("processing readability event",
		"articleId", evt.ArticleId,
		"url", evt.Url,
	)

	err = unitOfWork.Do(ctx, func(ctx context.Context) error {
		if err := useCase.HandleReadability(evt, ctx); err != nil {
			logger.Error("handle readability failed",
				"articleId", evt.ArticleId,
				"url", evt.Url,
				"error", err,
			)
			return err
		}
		return nil
	})

	if err != nil {
		logger.Warn("work unit failed, message will be requeued",
			"articleId", evt.ArticleId,
			"url", evt.Url,
			"error", err,
		)
	} else {
		logger.Info("work unit completed successfully",
			"articleId", evt.ArticleId,
			"url", evt.Url,
		)
	}

	return err
}

func assembleUseCase() *usescases.ArticleUseCase {
	httpClient := http.Client{
		Timeout: time.Second * 30,
	}

	repo := infrastructure.NewPostgresArticleRepository()
	read := infrastructure.NewReadabilityScraper(&httpClient, logger)
	art := usescases.NewArticleUseCase(read, repo)

	return art
}
