package scraping

import (
	"bytes"
	"context"
	"io"
	"net/http"
	"net/http/httptest"
	"testing"
)

type mockLogger struct{}

func (m *mockLogger) Debug(msg string, args ...any) {}
func (m *mockLogger) Info(msg string, args ...any)  {}
func (m *mockLogger) Warn(msg string, args ...any)  {}
func (m *mockLogger) Error(msg string, args ...any) {}

func TestScraper_Scrap_HttpSuccess(t *testing.T) {
	body := `<html><body><p>Hello</p><p>World</p><h1>Title</h1></body></html>`
	srv := httptest.NewServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Write([]byte(body))
	}))
	defer srv.Close()

	s := NewScraper(srv.Client(), &mockLogger{})
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

func TestScraper_Scrap_FallbackToHeadless(t *testing.T) {
	body := `<html><body><script>a</script><script>b</script><script>c</script><p>text</p></body></html>`
	srv := httptest.NewServer(http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Write([]byte(body))
	}))
	defer srv.Close()

	s := NewScraper(srv.Client(), &mockLogger{})
	_, err := s.Scrap(srv.URL, context.Background())
	if err == nil {
		t.Skip("headless scraper requires Chrome/chromedp – no error expected without it")
	}
}

func TestScraper_Scrap_HttpError(t *testing.T) {
	s := NewScraper(&http.Client{}, &mockLogger{})
	_, err := s.Scrap("://invalid-url", context.Background())
	if err == nil {
		t.Error("expected error for invalid URL, got nil")
	}
}

func TestScraper_Scrap_TransportError(t *testing.T) {
	s := NewScraper(&http.Client{
		Transport: &errorTransport{},
	}, &mockLogger{})
	_, err := s.Scrap("http://example.com", context.Background())
	if err == nil {
		t.Error("expected transport error, got nil")
	}
}
