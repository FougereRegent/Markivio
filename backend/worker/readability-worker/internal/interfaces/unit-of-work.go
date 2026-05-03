package interfaces

import "context"

type UnitOfWork interface {
	Commit(ctx context.Context)
	Rollback(ctx context.Context)
	CommitAndRestart(ctx context.Context)
}
