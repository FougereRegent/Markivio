package infrastructure

import (
	"context"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
)

type PostgresArticleRepository struct {
}

func NewPostgresArticleRepository() *PostgresArticleRepository {
	return &PostgresArticleRepository{
	}
}


func (a *PostgresArticleRepository) UpdateArticleUrl(ctx context.Context, article *domain.Article) (*domain.Article, error) {
	return nil, nil
}
