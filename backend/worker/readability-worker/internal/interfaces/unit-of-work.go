package interfaces

import "context"


// In do callback passing your logic or other works
type DoCallback func(ctx context.Context) error

type UnitOfWork interface {
	Do(ctx context.Context, callback DoCallback) error
}
