package interfaces

import (
	"context"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
)


type ArticleRepository interface {
	UpdateArticleUrl(ctx context.Context, article *domain.Article) (*domain.Article, error)
}
