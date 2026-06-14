package interfaces

import (
	"context"
	"io"
)

type Readability interface {
	ConvertWebSiteToMarkdown(url string, ctx context.Context) (io.Reader, error)
}

