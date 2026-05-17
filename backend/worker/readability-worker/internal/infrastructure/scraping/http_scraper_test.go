package scraping

import (
	"bytes"
	"compress/gzip"
	"context"
	"io"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/gkampitakis/go-snaps/snaps"
)

func TestCommonHeaders(t *testing.T) {
	header := commonHeaders
	snaps.MatchSnapshot(t, header)
}

func TestNewRequest(t *testing.T) {
	url := "https://example.com/article"
	req, err := newRequest(url)
	if err != nil {
		t.Fatalf("newRequest() unexpected error: %v", err)
	}

	if req.URL.String() != url {
		t.Errorf("URL = %s, want %s", req.URL.String(), url)
	}

	if req.Method != http.MethodGet {
		t.Errorf("Method = %s, want %s", req.Method, http.MethodGet)
	}

	for key, expected := range commonHeaders {
		got := req.Header.Get(key)
		if got != expected {
			t.Errorf("Header[%s] = %s, want %s", key, got, expected)
		}
	}
}

func TestNewRequest_InvalidURL(t *testing.T) {
	_, err := newRequest("://invalid-url")
	if err == nil {
		t.Error("expected error for invalid URL, got nil")
	}
}

func TestHttpScrapper_Scrap_Uncompressed(t *testing.T) {
	body := `<html><body><p>Hello world</p></body></html>`
	srv := httptest.NewServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Write([]byte(body))
	}))
	defer srv.Close()

	s := NewHttpScrapper(srv.Client())
	reader, err := s.Scrap(srv.URL, context.Background())
	if err != nil {
		t.Fatalf("Scrap() unexpected error: %v", err)
	}

	data, err := io.ReadAll(reader)
	if err != nil {
		t.Fatalf("ReadAll() unexpected error: %v", err)
	}

	if !bytes.Equal(data, []byte(body)) {
		t.Errorf("body = %s, want %s", string(data), body)
	}
}

func TestHttpScrapper_Scrap_GzipCompressed(t *testing.T) {
	body := `<html><body><p>Compressed content</p></body></html>`
	srv := httptest.NewServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Encoding", "gzip")
		gz := gzip.NewWriter(w)
		gz.Write([]byte(body))
		gz.Close()
	}))
	defer srv.Close()

	s := NewHttpScrapper(srv.Client())
	reader, err := s.Scrap(srv.URL, context.Background())
	if err != nil {
		t.Fatalf("Scrap() unexpected error: %v", err)
	}

	data, err := io.ReadAll(reader)
	if err != nil {
		t.Fatalf("ReadAll() unexpected error: %v", err)
	}

	if !bytes.Equal(data, []byte(body)) {
		t.Errorf("body = %s, want %s", string(data), body)
	}
}

func TestHttpScrapper_Scrap_ServerError(t *testing.T) {
	srv := httptest.NewServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.WriteHeader(http.StatusInternalServerError)
	}))
	defer srv.Close()

	s := NewHttpScrapper(srv.Client())
	_, err := s.Scrap(srv.URL, context.Background())
	if err != nil {
		t.Fatalf("Scrap() should not return error on 500 (HTTP response is still valid): %v", err)
	}
}

func TestHttpScrapper_Scrap_InvalidURL(t *testing.T) {
	s := NewHttpScrapper(http.DefaultClient)
	_, err := s.Scrap("://invalid", context.Background())
	if err == nil {
		t.Error("expected error for invalid URL, got nil")
	}
}

func TestHttpScrapper_Scrap_TransportError(t *testing.T) {
	s := NewHttpScrapper(&http.Client{
		Transport: &errorTransport{},
	})
	_, err := s.Scrap("http://example.com", context.Background())
	if err == nil {
		t.Error("expected transport error, got nil")
	}
}

type errorTransport struct{}

func (t *errorTransport) RoundTrip(*http.Request) (*http.Response, error) {
	return nil, http.ErrAbortHandler
}
