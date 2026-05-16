package infrastructure

import (
	"bytes"
	"io"
	"net/http"
	"net/url"

	"codeberg.org/readeck/go-readability/v2"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure/scraping"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/uncompress"
)

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
	httpScrapper := scraping.NewHttpScrapper(r.client)
	reader, err := httpScrapper.DoRequest(urlSite)
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
