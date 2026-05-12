package infrastructure

import (
	"context"
	"sync"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces"
	"github.com/jackc/pgx/v5"
)

const (
	TransactionKey string = "transaction"
)

type PgIface interface {
	Begin(ctx context.Context) (pgx.Tx, error)
	BeginTx(ctx context.Context, opts pgx.TxOptions) (pgx.Tx, error)
}

type unitOfWork struct {
	connection PgIface
	transaction pgx.Tx
	mu sync.Mutex
}

func NewUnitOfWork(connection PgIface) interfaces.UnitOfWork {
	return &unitOfWork{
		connection: connection,
		transaction: nil,
		mu: sync.Mutex{},
	}
}

func (u *unitOfWork) Do(ctx context.Context, callback interfaces.DoCallback) error {
	var err error
	conn := u.connection
	mu := &u.mu

	mu.Lock()
	defer mu.Unlock()

	if u.transaction == nil {
		u.transaction, err = conn.BeginTx(ctx, pgx.TxOptions{
			IsoLevel: pgx.ReadCommitted,
		})
		if err != nil {
			return err
		}
	}

	defer func() {
		u.transaction = nil
	}()
	defer u.transaction.Rollback(ctx)

	newCtx := context.WithValue(ctx, TransactionKey, &u.transaction)
	if err = callback(newCtx); err == nil {
		u.transaction.Commit(newCtx)
	} else {
		return err
	}

	return nil
}
