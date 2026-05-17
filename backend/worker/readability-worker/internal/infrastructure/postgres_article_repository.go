package infrastructure

import (
	"context"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/uow"
	"github.com/jackc/pgx/v5"
)

type PostgresArticleRepository struct {
}

func NewPostgresArticleRepository() *PostgresArticleRepository {
	return &PostgresArticleRepository{
	}
}


func (a *PostgresArticleRepository) UpdateArticleContent(ctx context.Context, article *domain.Article) (*domain.Article, error) {
	transation := *(ctx.Value(uow.TransactionKey).(*pgx.Tx))
	_ , err := transation.Exec(ctx, "UPDATE articles SET \"articleContent_Content\"=$1 WHERE id=$2", article.ArticleContent, article.Id.String())
	if err != nil {
		return nil, err
	}
	return article, nil
}
