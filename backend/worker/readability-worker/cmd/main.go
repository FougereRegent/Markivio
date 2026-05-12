package main

import (
	"context"
	"fmt"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/logger"
	"github.com/jackc/pgx/v5/pgxpool"
)

type EnvName string

const (
	PgUsernameEnv EnvName = "WORKER_PG_USERNAME"
	PgPasswordEnv EnvName = "WORKER_PG_PASSWORD"
	PgHostEnv EnvName = "WORKER_PG_HOST"
	PgPortEnv EnvName = "WORKER_PG_PORT"
	PgDatabase    EnvName = "WORKER_PG_DB"
	MqUserEnv     EnvName = "WORKER_MQ_USER"
	MqPasswordEnv EnvName = "WORKER_MQ_PASSWORD"
	MqHostEnv EnvName = "WORKER_MQ_HOST"
	MqPortEnv EnvName = "WORKER_MQ_PORT"
)

type Config struct {
	PgUsername string
	PgPassword string
	PgPort string
	PgHost string
	PgDb string
	MqUser     string
	MqPassword string
	MqHost string
	MqPort string
}

var config Config

func main() {
	pgpool := initDb()
	defer pgpool.Close()

	if err := pgpool.Ping(context.Background()); err != nil {
		logger.PanicIfError(err, "Cannot ping postgres")
	}
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

func initLogger() {

}

func initReadabilityHandler() {

}
