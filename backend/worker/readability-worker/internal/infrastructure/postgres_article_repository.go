package infrastructure

import (
	"context"
	"errors"
	"fmt"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/uow"
	"github.com/jackc/pgx/v5"
)

type PostgresArticleRepository struct {
}

func NewPostgresArticleRepository() *PostgresArticleRepository {
	return &PostgresArticleRepository{}
}

func (a *PostgresArticleRepository) UpdateArticleContent(ctx context.Context, article *domain.Article) (*domain.Article, error) {
	transaction := *(ctx.Value(uow.TransactionKey).(*pgx.Tx))
	ct, err := transaction.Exec(ctx, "UPDATE articles SET \"articleContent_Content\"=$1 WHERE id=$2", article.ArticleContent, article.Id.String())
	if err != nil {
		return nil, domain.NewDatabaseError("failed to update article content", fmt.Errorf("db exec: %w", err))
	}
	if ct.RowsAffected() == 0 {
		return nil, domain.NewArticleNotFoundError(article.Id)
	}
	if errors.Is(err, pgx.ErrNoRows) {
		return nil, domain.NewArticleNotFoundError(article.Id)
	}
	return article, nil
}
