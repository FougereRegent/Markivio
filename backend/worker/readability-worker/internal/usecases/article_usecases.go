package usescases

import (
	"context"
	"io"
	"log/slog"

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

func (a *ArticleUseCase) HandleReadability(createReadability domain.CreateReadabilityEvt, ctx context.Context) (error) {
	siteReader, err := a.read.ConvertWebSiteToMarkdown(createReadability.Url, ctx)
	if err != nil {
		return err
	}

	content, err := io.ReadAll(siteReader)
	if err != nil {
		return err
	}

	slog.Info(string(content))
	return nil
}

