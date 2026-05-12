package infrastructure

import (
	"bytes"
	"io"
	"net/http"
	"net/url"

	"codeberg.org/readeck/go-readability/v2"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/uncompress"
)

type Headers map[string]string

const (
	urlTimeout = 30
)

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

type ReadabilityScraper struct {
	client *http.Client
}

func NewReadabilityScraper(client *http.Client) *ReadabilityScraper {
	return &ReadabilityScraper{
		client: client,
	}
}

func (r *ReadabilityScraper) ConvertWebSiteToMarkdown(urlSite string) (io.Reader, error) {
	var b bytes.Buffer
	req, err := newRequest(urlSite)
	if err != nil {
		return nil, err
	}
	reader, err := r.doRequest(req)
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

func newRequest(url string) (*http.Request, error) {
	req, err := http.NewRequest("GET", url, nil)
	if err != nil {
		return nil, err
	}
	for key, val := range commonHeaders {
		req.Header.Set(key, val)
	}
	return req, nil
}

func (r *ReadabilityScraper) doRequest(req *http.Request) (io.Reader, error) {
	resp, err := r.client.Do(req)
	if err != nil {
		return nil, err
	}

	compressHeader := resp.Header.Get("content-encoding")
	if compressHeader == "" {
		return resp.Body, nil
	}

	uncomp := uncompress.New(uncompress.CompressType(compressHeader))
	return uncomp.Uncompress(resp.Body)
}
