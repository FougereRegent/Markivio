package interfaces

import (
	"context"
	"testing"
	"github.com/stretchr/testify/mock"
)

type TxMock struct {
	mock.Mock
}

func (t *TxMock) Rollback(ctx context.Context) error {
	args := t.Called(ctx)
	return args.Error(0)
}

func (t *TxMock) Commit(ctx context.Context) error {
	args := t.Called(ctx)
	return args.Error(0)
}

func TestBeginTransationWithTransaction(t *testing.T) {

}

