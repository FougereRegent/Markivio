package usescases

import (
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces"
)

type ArticleUseCase struct {
	read interfaces.Readability
	articleRepo interfaces.ArticleRepository
}

func NewArticleUseCase(read interfaces.Readability, repo interfaces.ArticleRepository) (*ArticleUseCase) {
	return &ArticleUseCase{
		read: read,
		articleRepo: repo,
	}
}

func (a *ArticleUseCase) HandleReadability(createReadability domain.CreateReadabilityEvt) (error) {
	return nil
}

