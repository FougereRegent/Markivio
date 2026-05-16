package scraping

import "io"

type Scrapper interface{
	DoRequest(url string) (io.Reader, error)
}
