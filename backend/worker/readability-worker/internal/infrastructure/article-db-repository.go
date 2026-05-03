package infrastructure

import (
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/jackc/pgx/v5/pgxpool"
)

type ArticleDbRepository struct {
	pool *pgxpool.Pool
}

func NewArticleDbRepository(pool *pgxpool.Pool) (*ArticleDbRepository) {
	return &ArticleDbRepository{
		pool: pool,
	}
}

func (a *ArticleDbRepository) Update(article *domain.Article) (*domain.Article, error) {
	return nil, nil
}
