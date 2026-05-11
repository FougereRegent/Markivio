package infrastructure

import (
	"context"
	"errors"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces"
	"github.com/jackc/pgx/v5"
)

type unitOfWork struct {
	connection *pgx.Conn
	transaction pgx.Tx
}

func NewUnitOfWork(connection *pgx.Conn) interfaces.UnitOfWork {
	return &unitOfWork{
		connection: connection,
		transaction: nil,
	}
}

func (u *unitOfWork) BeginTransaction(ctx context.Context) error {
	var err error
	if u.transaction != nil {
		u.transaction.Rollback(ctx)
		u.transaction = nil
	}
	u.transaction, err = u.connection.BeginTx(ctx, pgx.TxOptions{
		IsoLevel: pgx.ReadCommitted,
	})
	return err
}

func (u *unitOfWork) Commit(ctx context.Context) error {
	return u.atomicOperation(func (tx pgx.Tx, ctx context.Context) error  {
		return tx.Commit(ctx)
	}, ctx)
}

func (u *unitOfWork) Rollback(ctx context.Context) error {
	return u.atomicOperation(func (tx pgx.Tx, ctx context.Context) error {
		return tx.Rollback(ctx)
	}, ctx)
}

func (u *unitOfWork) atomicOperation(op func (tx pgx.Tx, ctx context.Context) error , ctx context.Context) error {
	tx := u.transaction
	if tx == nil {
		return errors.New("Can't have transaction")
	}
	if err := op(tx, ctx); err != nil {
		u.transaction = nil
		return err
	}
	u.transaction = nil
	return nil
}
