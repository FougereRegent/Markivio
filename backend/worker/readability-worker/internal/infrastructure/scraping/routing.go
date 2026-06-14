package scraping

import (
	"bytes"
	"context"
	"fmt"
	"io"
	"net/http"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
)

type IScraper interface {
	Scrap(url string, ctx context.Context) (io.Reader, error)
}
type Scraper struct {

	httpClient *http.Client
	logger logger.ILog
}

func NewScraper(httpClient *http.Client, logger logger.ILog) IScraper {
	return &Scraper{
		httpClient: httpClient,
		logger: logger,
	}
}

func (s *Scraper) Scrap(url string, ctx context.Context) (io.Reader, error) {
	var scraper IScraper

	scraper = NewHttpScrapper(s.httpClient)
	reader, err := scraper.Scrap(url, ctx)
	if err != nil {
		return nil, fmt.Errorf("http scrape: %w", err)
	}

	data, err := io.ReadAll(reader)
	if err != nil {
		return nil, fmt.Errorf("read http response: %w", err)
	}

	scoringReader := bytes.NewReader(data)

	score, err := scoring(scoringReader)
	if err != nil {
		return nil, fmt.Errorf("scoring HTML: %w", err)
	}

	scoringReader.Seek(0, 0)
	if score.ScriptRatio < score.TextRatio {
		return scoringReader, nil
	}

	s.logger.Info("http scraper returned script-heavy page, falling back to headless",
		"url", url,
		"scriptRatio", score.ScriptRatio,
		"textRatio", score.TextRatio,
	)

	scraper = NewHeadlessScraper()

	headlessReader, err := scraper.Scrap(url, ctx)
	if err != nil {
		return nil, domain.NewScrapingFailedError(url, fmt.Errorf("headless fallback: %w", err))
	}

	return headlessReader, nil
}
