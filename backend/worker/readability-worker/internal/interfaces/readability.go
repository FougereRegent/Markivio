package interfaces

import "io"

type Readability interface {
	ConvertWebSiteToMarkdown(url string) (io.Reader, error)
}

