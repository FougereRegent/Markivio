package main

import (
	"context"
	"encoding/json"
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
		helplog.PanicIfError(err, "Cannot ping postgres")
	}

	w, err := worker.NewWorker(logger, worker.WorkerOpts{
		PoolSize:    10,
		RabbitMqUri: rabbitMqUri,
		QeueuName: "readability-worker",
	})

	if err != nil {
		helplog.PanicIfError(err, "Cannot create worker")
	}

	forever := make(chan struct{})

	//Main loop
	w.Run(work)

	<-forever
}

func work(data string, ctx context.Context) error {
	useCase := assembleUseCase()
	unitOfWork := uow.NewUnitOfWork(pgpool, logger)

	var evt domain.CreateReadabilityEvt

	err := json.Unmarshal([]byte(data), &evt)
	if err != nil {
		return nil
	}

	unitOfWork.Do(ctx, func(ctx context.Context) error {
		return useCase.HandleReadability(evt)
	})

	return nil
}

func assembleUseCase() *usescases.ArticleUseCase {
	httpClient := http.Client{
		Timeout: time.Second * 30,
	}

	repo := infrastructure.NewPostgresArticleRepository()
	read := infrastructure.NewReadabilityScraper(&httpClient)
	art := usescases.NewArticleUseCase(read, repo)

	return art
}
