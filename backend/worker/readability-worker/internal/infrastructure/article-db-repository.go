package infrastructure

import "github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"

type ArticleDbRepository struct {

}

func New() (*ArticleDbRepository) {
	return nil
}

func (a *ArticleDbRepository) Update(article *domain.Article) (*domain.Article, error) {
	return nil, nil
}
