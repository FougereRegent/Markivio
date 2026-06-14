package scraping

import (
	"bytes"
	"context"
	"io"
	"time"

	"github.com/chromedp/chromedp"
)

type HeadlessScraper struct {
}

func NewHeadlessScraper() IScraper {
	return &HeadlessScraper{}
}

func (h *HeadlessScraper) Scrap(url string, ctx context.Context) (io.Reader, error) {
	if ctx == nil {
		ctx = context.Background()
	}
	// IMPORTANT: un contexte Chrome par goroutine
	ctx, cancelTimeout := context.WithTimeout(ctx, 25*time.Second)
	ctx, cancel := chromedp.NewContext(ctx)
	defer cancel()

	defer cancelTimeout()

	var html string
	err := chromedp.Run(ctx,
		chromedp.Navigate(url),
		chromedp.Sleep(time.Second*2),
		chromedp.OuterHTML("html", &html),
	)
	if err != nil {
		return nil, err
	}

	return bytes.NewReader([]byte(html)), nil
}
