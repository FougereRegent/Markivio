package scraping

import (
	"context"
	"testing"
	"time"
)

func TestHeadlessScraper_Scrap_CancelledContext(t *testing.T) {
	ctx, cancel := context.WithCancel(context.Background())
	cancel()

	h := NewHeadlessScraper()
	_, err := h.Scrap("https://example.com", ctx)
	if err == nil {
		t.Error("expected error for cancelled context, got nil")
	}
}

func TestHeadlessScraper_Scrap_TimeoutContext(t *testing.T) {
	ctx, cancel := context.WithTimeout(context.Background(), -1*time.Second)
	defer cancel()

	h := NewHeadlessScraper()
	_, err := h.Scrap("https://example.com", ctx)
	if err == nil {
		t.Error("expected error for timed-out context, got nil")
	}
}
