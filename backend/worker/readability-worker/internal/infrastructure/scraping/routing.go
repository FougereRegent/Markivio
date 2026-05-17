package scraping

import (
	"bytes"
	"context"
	"io"
	"net/http"

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
	reader, err := scraper.Scrap(url, nil)
	if err != nil {
		return nil, err
	}

	data, _ := io.ReadAll(reader)
	scoringReader := bytes.NewReader(data)

	score, err := scoring(scoringReader)
	if err != nil {
		return nil, err
	}

	scoringReader.Seek(0,0)
	if score.ScriptRatio < score.TextRatio {
		return scoringReader, nil
	} else {
		scraper = NewHeadlessScraper()
	}

	return scraper.Scrap(url, ctx)
}
