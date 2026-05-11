package infrastructure

import (
	"context"
	"testing"

	"github.com/pashagolub/pgxmock/v5"
)

func callbackSuccess(ctx context.Context) error {
	return nil
}

func TestUnitOfWorkShouldCommit(t *testing.T) {
	mock, err := pgxmock.NewConn()
	if err != nil {
		t.Fatal(err)
	}

	unitOfWork := NewUnitOfWork(mock)

	unitOfWork.Do(context.Background(), callbackSuccess)
}

func TestUnitOfWorkShouldRollback(t *testing.T) {

}
