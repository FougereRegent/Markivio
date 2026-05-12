package main

import (
	"context"
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/usecases"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/logger"
	"github.com/google/uuid"
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
)

type Config struct {
	PgUsername string
	PgPassword string
	PgPort string
	PgHost string
	PgDb string
	MqUser     string
	MqPassword string
}

var config Config

func init() {
	config = Config{
		PgUsername: os.Getenv(string(PgUsernameEnv)),
		PgPassword: os.Getenv(string(PgPasswordEnv)),
		PgHost: os.Getenv(string(PgHostEnv)),
		PgPort: os.Getenv(string(PgPortEnv)),
		MqUser:     os.Getenv(string(MqUserEnv)),
		MqPassword: os.Getenv(string(MqPasswordEnv)),
	}
}

func main() {
	pgpool := initDb()

	artRepo := infrastructure.NewPostgresArticleRepository(pgpool)
	artReadbility := infrastructure.NewReadabilityScraper(&http.Client{})
	artUsecase := usescases.NewArticleUseCase(
		artReadbility,
		artRepo,
	)

	err := artUsecase.HandleReadability(domain.CreateReadabilityEvt{
		ArticleId: uuid.UUID{},
		Url:       os.Args[1],
	})

	if err != nil {
		log.Panicf("%s", err)
	}
}

func initDb() *pgxpool.Pool {
	dsn := fmt.Sprintf("postgres://%s:%s@%s:%d/%s",
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
