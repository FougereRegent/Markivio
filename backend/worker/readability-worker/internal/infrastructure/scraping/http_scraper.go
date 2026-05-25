package scraping

import (
	"context"
	"fmt"
	"io"
	"net/http"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/uncompress"
)

type HttpScrapper struct {
	httpClient *http.Client
}

type Headers map[string]string

var commonHeaders = Headers{
	"Accept":          "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
	"Accept-Encoding": "gzip, deflate",
	"Accept-Language": "fr-FR,fr;q=0.9,en;q=0.8",
	"User-Agent":      "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
	"Connection":      "keep-alive",
	"Upgrade-Insecure-Requests": "1",
	"Sec-Fetch-Dest":  "document",
	"Sec-Fetch-Mode":  "navigate",
	"Sec-Fetch-Site":  "none",
	"Sec-Fetch-User":  "?1",
	"Cache-Control":   "max-age=0",
}

func NewHttpScrapper(client *http.Client) IScraper {
	return &HttpScrapper{
		httpClient: client,
	}
}

func newRequest(url string) (*http.Request, error) {
	req, err := http.NewRequest("GET", url, nil)
	if err != nil {
		return nil, fmt.Errorf("newRequest: %w", err)
	}
	for key, val := range commonHeaders {
		req.Header.Set(key, val)
	}
	return req, nil
}

func (s *HttpScrapper) Scrap(url string, ctx context.Context) (io.Reader, error) {
	request, err := newRequest(url)
	if err != nil {
		return nil, domain.NewInvalidURLError(url, err)
	}

	resp, err := s.httpClient.Do(request)
	if err != nil {
		return nil, fmt.Errorf("http request: %w", err)
	}

	if resp.StatusCode >= 400 {
		resp.Body.Close()
		return nil, domain.NewHTTPStatusError(url, resp.StatusCode)
	}

	compressHeader := resp.Header.Get("content-encoding")
	if compressHeader == "" {
		return resp.Body, nil
	}

	uncomp, err := uncompress.New(uncompress.CompressType(compressHeader))
	if err != nil {
		resp.Body.Close()
		return nil, fmt.Errorf("decompress %s: %w", compressHeader, err)
	}

	reader, err := uncomp.Uncompress(resp.Body)
	if err != nil {
		resp.Body.Close()
		return nil, fmt.Errorf("uncompress body: %w", err)
	}
	return reader, nil
}
