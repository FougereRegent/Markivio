package scraping

import "io"

type HeadlessScraper struct {

}

func NewHeadlessScraper() IScraper {
	return &HeadlessScraper{}
}

func (h *HeadlessScraper) Scrap(url string) (io.Reader, error) {
	return nil, nil
}
