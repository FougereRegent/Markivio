package main

import (
	"context"
	"fmt"
	"log"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/worker"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/logger"
	"github.com/jackc/pgx/v5/pgxpool"
)

type EnvName string

const (
	PgUsernameEnv EnvName = "WORKER_PG_USERNAME"
	PgPasswordEnv EnvName = "WORKER_PG_PASSWORD"
	PgHostEnv     EnvName = "WORKER_PG_HOST"
	PgPortEnv     EnvName = "WORKER_PG_PORT"
	PgDatabase    EnvName = "WORKER_PG_DB"
	MqUserEnv     EnvName = "WORKER_MQ_USER"
	MqPasswordEnv EnvName = "WORKER_MQ_PASSWORD"
	MqHostEnv     EnvName = "WORKER_MQ_HOST"
	MqPortEnv     EnvName = "WORKER_MQ_PORT"
)

type Config struct {
	PgUsername string
	PgPassword string
	PgPort     string
	PgHost     string
	PgDb       string
	MqUser     string
	MqPassword string
	MqHost     string
	MqPort     string
}

var config Config

func main() {
	ctx := context.Background()
	pgpool := initDb()
	defer pgpool.Close()

	if err := pgpool.Ping(ctx); err != nil {
		logger.PanicIfError(err, "Cannot ping postgres")
	}

	rabbitMqUri := worker.UriBuilder(worker.RabbitMqConn{
		User:     config.MqUser,
		Password: config.MqPassword,
		Host:     config.MqHost,
		Port:     config.MqPort,
	},
	)

	w, err := worker.NewWorker(nil, worker.WorkerOpts{
		PoolSize:    10,
		RabbitMqUri: rabbitMqUri,
		QeueuName:   "readability-worker",
	})

	if err != nil {
		logger.PanicIfError(err, "Cannot create worker")
	}

	forever := make(chan struct{})
	w.Run(func(data string, ctx context.Context) error {
		log.Println(data)
		return nil
	})
	<-forever
}

func initDb() *pgxpool.Pool {
	dsn := fmt.Sprintf("postgres://%s:%s@%s:%s/%s",
		config.PgUsername,
		config.PgPassword,
		config.PgHost,
		config.PgPort,
		config.PgDb,
	)
	pool, err := pgxpool.New(context.Background(), dsn)
	logger.PanicIfError(err, "Cannot connect to psql please check your creds")

	return pool
}
