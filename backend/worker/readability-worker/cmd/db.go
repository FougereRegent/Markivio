package main

import (
	"context"
	"fmt"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/helplog"
	"github.com/jackc/pgx/v5/pgxpool"
)

func initDb() *pgxpool.Pool {
	dsn := fmt.Sprintf("postgres://%s:%s@%s:%s/%s",
		config.PgUsername,
		config.PgPassword,
		config.PgHost,
		config.PgPort,
		config.PgDb,
	)
	pool, err := pgxpool.New(context.Background(), dsn)
	helplog.PanicIfError(err, "Cannot connect to psql please check your creds")

	return pool
}
