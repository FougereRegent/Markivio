package infrastructure

import (
	"bytes"
	"io"
	"net/http"
	"net/url"

	"codeberg.org/readeck/go-readability/v2"
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

func (r *ReadabilityScraper) ConvertWebSiteToMarkdown(urlSite string) (io.Reader, error) {
	var b bytes.Buffer
	httpScrapper := scraping.NewScraper(r.client, r.logger)
	reader, err := httpScrapper.Scrap(urlSite)
	if err != nil {
		return nil, err
	}

	baseUrl, _ := url.Parse(urlSite)
	article, err := readability.FromReader(reader, baseUrl)
	if err != nil {
		return nil, err
	}
	article.RenderHTML(&b)
	return &b, nil
}
