package infrastructure

import (
	"context"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces"
	"github.com/jackc/pgx/v5/pgxpool"
)

type unitOfWork struct {

}

func NewUnitOfWork(pool *pgxpool.Pool) interfaces.UnitOfWork {
	return &unitOfWork{

	}
}

func (u *unitOfWork) Commit(ctx context.Context) {

}

func (u *unitOfWork) Rollback(ctx context.Context) {

}

func (u *unitOfWork) CommitAndRestart(ctx context.Context) {

}

