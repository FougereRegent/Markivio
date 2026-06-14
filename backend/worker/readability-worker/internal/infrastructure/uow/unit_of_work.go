package uow

import (
	"context"
	"errors"
	"fmt"
	"sync"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
	"github.com/jackc/pgx/v5"
)

const (
	TransactionKey string = "transaction"
)

type PgIface interface {
	Begin(ctx context.Context) (pgx.Tx, error)
	BeginTx(ctx context.Context, opts pgx.TxOptions) (pgx.Tx, error)
}

type DbConn interface {
	Query(ctx context.Context, query string, args ...any) (pgx.Rows, error)
}

type unitOfWork struct {
	connection  PgIface
	transaction pgx.Tx
	mu          sync.Mutex

	logger logger.ILog
}

func NewUnitOfWork(connection PgIface, logger logger.ILog) interfaces.UnitOfWork {
	return &unitOfWork{
		connection:  connection,
		transaction: nil,
		mu:          sync.Mutex{},
		logger:      logger,
	}
}

func (u *unitOfWork) Do(ctx context.Context, callback interfaces.DoCallback) error {
	var err error
	conn := u.connection
	mu := &u.mu

	mu.Lock()
	defer mu.Unlock()

	if u.transaction != nil {
		u.transaction.Rollback(ctx)
	}

	u.transaction, err = conn.BeginTx(context.Background(), pgx.TxOptions{
		IsoLevel: pgx.ReadCommitted,
	})
	if err != nil {
		return fmt.Errorf("unit-of-work: begin tx: %w", err)
	}

	defer func() {
		u.transaction = nil
	}()

	newCtx := context.WithValue(ctx, TransactionKey, &u.transaction)
	if err = callback(newCtx); err == nil {
		u.logger.Info("unit-of-work: commit transaction")
		if commitErr := u.transaction.Commit(context.Background()); commitErr != nil {
			return fmt.Errorf("unit-of-work: commit: %w", commitErr)
		}
		return nil
	}

	u.logger.Info("unit-of-work: rollback transaction")
	return errors.Join(
		fmt.Errorf("unit-of-work: callback: %w", err),
		u.transaction.Rollback(context.Background()),
	)
}
