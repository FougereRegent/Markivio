package interfaces

import "github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"


type ArticleRepository interface {
	Update(article *domain.Article) (*domain.Article, error)
}
