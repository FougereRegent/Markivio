package uow

import (
	"context"
	"errors"
	"testing"

	"github.com/jackc/pgx/v5"
	"github.com/pashagolub/pgxmock/v5"
)

type LogMock struct{}

func (l *LogMock)Debug(msg string, args ...any){}
func (l *LogMock)Info(msg string, args ...any){}
func (l *LogMock)Warn(msg string, args ...any){}
func (l *LogMock)Error(msg string, args ...any){}

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
	unitOfWork := NewUnitOfWork(mock, &LogMock{})

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
	unitOfWork := NewUnitOfWork(mock, &LogMock{})

	if err = unitOfWork.Do(context.Background(), callbackFailed); err == nil {
		t.Fatal()
	}

	if err = mock.ExpectationsWereMet(); err != nil {
		t.Fatal(err)
	}
}

func TestUnitOfWorkContextContainsTransaction(t *testing.T) {
	mock, err := pgxmock.NewConn()
	if err != nil {
		t.Fatal(err)
	}

	mock.ExpectBeginTx(pgx.TxOptions{IsoLevel: pgx.ReadCommitted})
	mock.ExpectCommit()
	uow := NewUnitOfWork(mock, &LogMock{})

	err = uow.Do(context.Background(), func(ctx context.Context) error {
		tx := ctx.Value(TransactionKey)
		if tx == nil {
			return errors.New("transaction not found in context")
		}
		_, ok := tx.(*pgx.Tx)
		if !ok {
			return errors.New("context value is not *pgx.Tx")
		}
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}

	if err = mock.ExpectationsWereMet(); err != nil {
		t.Fatal(err)
	}
}

func TestUnitOfWorkBeginTxError(t *testing.T) {
	mock, err := pgxmock.NewConn()
	if err != nil {
		t.Fatal(err)
	}

	expectedErr := errors.New("connection failed")
	mock.ExpectBeginTx(pgx.TxOptions{IsoLevel: pgx.ReadCommitted}).WillReturnError(expectedErr)
	uow := NewUnitOfWork(mock, &LogMock{})

	err = uow.Do(context.Background(), callbackSuccess)
	if !errors.Is(err, expectedErr) {
		t.Fatalf("expected %v, got %v", expectedErr, err)
	}

	if err = mock.ExpectationsWereMet(); err != nil {
		t.Fatal(err)
	}
}

func TestUnitOfWorkTransactionResetAfterDo(t *testing.T) {
	mock, err := pgxmock.NewConn()
	if err != nil {
		t.Fatal(err)
	}

	mock.ExpectBeginTx(pgx.TxOptions{IsoLevel: pgx.ReadCommitted})
	mock.ExpectCommit()
	mock.ExpectBeginTx(pgx.TxOptions{IsoLevel: pgx.ReadCommitted})
	mock.ExpectCommit()
	uow := NewUnitOfWork(mock, &LogMock{})

	if err = uow.Do(context.Background(), callbackSuccess); err != nil {
		t.Fatal(err)
	}
	if err = uow.Do(context.Background(), callbackSuccess); err != nil {
		t.Fatal(err)
	}

	if err = mock.ExpectationsWereMet(); err != nil {
		t.Fatal(err)
	}
}
