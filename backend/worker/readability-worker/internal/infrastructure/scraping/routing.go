package scraping

import (
	"bytes"
	"errors"
	"io"
	"net/http"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
)

type IScraper interface {
	Scrap(url string) (io.Reader, error)
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

func (s *Scraper) Scrap(url string) (io.Reader, error) {
	var scraper IScraper

	scraper = NewHttpScrapper(s.httpClient)
	reader, err := scraper.Scrap(url)
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
		s.logger.Warn("Headless Scrapping")
		return nil, errors.New("Not Implemented")
	}
}
