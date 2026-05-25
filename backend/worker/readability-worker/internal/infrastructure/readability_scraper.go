package infrastructure

import (
	"bytes"
	"context"
	"fmt"
	"io"
	"net/http"
	"net/url"

	"codeberg.org/readeck/go-readability/v2"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/scraping"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
)

type ReadabilityScraper struct {
	client *http.Client
	logger logger.ILog
}

func NewReadabilityScraper(client *http.Client, logger logger.ILog) *ReadabilityScraper {
	return &ReadabilityScraper{
		client: client,
		logger: logger,
	}
}

func (r *ReadabilityScraper) ConvertWebSiteToMarkdown(urlSite string, ctx context.Context) (io.Reader, error) {
	var b bytes.Buffer
	httpScrapper := scraping.NewScraper(r.client, r.logger)
	reader, err := httpScrapper.Scrap(urlSite, ctx)
	if err != nil {
		r.logger.Error("scraping failed",
			"url", urlSite,
			"error", err,
		)
		return nil, fmt.Errorf("scrap %s: %w", urlSite, err)
	}

	baseUrl, err := url.Parse(urlSite)
	if err != nil {
		return nil, domain.NewInvalidURLError(urlSite, err)
	}

	article, err := readability.FromReader(reader, baseUrl)
	if err != nil {
		r.logger.Error("readability extraction failed",
			"url", urlSite,
			"error", err,
		)
		return nil, fmt.Errorf("readability.FromReader(%s): %w", urlSite, err)
	}
	article.RenderHTML(&b)
	r.logger.Info("successfully converted article",
		"url", urlSite,
		"title", article.Title,
		"contentLength", b.Len(),
	)
	return &b, nil
}
