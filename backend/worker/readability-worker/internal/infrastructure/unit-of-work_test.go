package infrastructure

import (
	"context"
	"errors"
	"testing"

	"github.com/jackc/pgx/v5"
	"github.com/pashagolub/pgxmock/v5"
)

func callbackSuccess(ctx context.Context) error {
	return nil
}

func callbackFailed(ctx context.Context) error {
	return errors.New("Err")
}

func TestUnitOfWorkShouldCommit(t *testing.T) {
	mock, err := pgxmock.NewConn()
	if err != nil {
		t.Fatal(err)
	}

	mock.ExpectBeginTx(pgx.TxOptions{
		IsoLevel: pgx.ReadCommitted,
	})
	mock.ExpectCommit()
	unitOfWork := NewUnitOfWork(mock)

	unitOfWork.Do(context.Background(), callbackSuccess)

	if err = mock.ExpectationsWereMet(); err != nil {
		t.Fatal(err)
	}
}

func TestUnitOfWorkShouldRollback(t *testing.T) {
	mock, err := pgxmock.NewConn()
	if err != nil {
		t.Fatal(err)
	}

	mock.ExpectBeginTx(pgx.TxOptions{
		IsoLevel: pgx.ReadCommitted,
	})
	mock.ExpectRollback().Maybe().Times(1)
	mock.ExpectCommit().Maybe().Times(0)
	unitOfWork := NewUnitOfWork(mock)

	if err = unitOfWork.Do(context.Background(), callbackFailed); err == nil {
		t.Fatal()
	}

	if err = mock.ExpectationsWereMet(); err != nil {
		t.Fatal(err)
	}
}
