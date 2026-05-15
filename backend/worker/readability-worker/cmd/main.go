package main

import (
	"context"
	"log"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/worker"
	mylogger "github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
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
	w.Run(func(data string, ctx context.Context) error {
		log.Println(data)
		return nil
	})

	<-forever
}
