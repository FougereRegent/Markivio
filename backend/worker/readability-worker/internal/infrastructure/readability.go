package infrastructure

import (
	"bytes"
	"io"
	"net/http"
	"net/url"

	"codeberg.org/readeck/go-readability/v2"
	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/pkg/uncompress"
)

type headers map[string]string
const (
	url_timeout = 30
)

var commonHeaders headers = headers{
	"Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
	"Accept-Encoding": "gzip, deflate",
	"Accept-Language": "fr-FR,fr;q=0.9,en;q=0.8",
	"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
	"Connection": "keep-alive",
	"Upgrade-Insecure-Requests": "1",
	"Sec-Fetch-Dest": "document",
	"Sec-Fetch-Mode": "navigate",
	"Sec-Fetch-Site": "none",
	"Sec-Fetch-User": "?1",
	"Cache-Control": "max-age=0",
}

type WebSiteReadability struct {
	client *http.Client
}

func NewReadability(client *http.Client) WebSiteReadability {
	return WebSiteReadability{
		client: client,
	}
}

func (w *WebSiteReadability) ConvertWebSiteToMarkdown(urlSite string) (io.Reader, error) {
	var b bytes.Buffer
	req, err := newRequest(urlSite)
	if err != nil {
		return nil, err
	}
	reader, err := w.doRequest(req)
	if err != nil {
		return nil, err
	}

	baseUrl, _ := url.Parse(urlSite)
	article, err := readability.FromReader(reader, baseUrl)
	article.RenderHTML(&b)
	return &b, nil
}

func newRequest(url string) (*http.Request, error) {
	req, err := http.NewRequest("GET", url, nil)
	for key, val := range commonHeaders {
		req.Header.Set(key,val);
	}
	return req, err
}

func (w *WebSiteReadability) doRequest(req *http.Request) (io.Reader, error) {
	client := w.client
	resp, err := client.Do(req)
	if err != nil {
		return nil, err
	}

	if compressHeader := resp.Header.Get("content-encoding"); compressHeader == "" {
		return resp.Body, nil
	} else {
		uncomp := uncompress.New(uncompress.COMPRESS_TYPE(compressHeader))
		return uncomp.Uncompress(resp.Body)
	}
}
