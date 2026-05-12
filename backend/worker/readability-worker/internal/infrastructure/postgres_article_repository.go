package infrastructure

import (
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/jackc/pgx/v5/pgxpool"
)

type PostgresArticleRepository struct {
	pool *pgxpool.Pool
}

func NewPostgresArticleRepository(pool *pgxpool.Pool) *PostgresArticleRepository {
	return &PostgresArticleRepository{
		pool: pool,
	}
}

func (a *PostgresArticleRepository) Update(article *domain.Article) (*domain.Article, error) {
	return nil, nil
}
