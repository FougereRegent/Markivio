package uncompress

import (
	"compress/bzip2"
	"compress/flate"
	"compress/gzip"
	"fmt"
	"io"
)

type CompressType string

const (
	Gzip    CompressType = "gzip"
	Deflate CompressType = "deflate"
	BR      CompressType = "br"
)

var ErrUnknownCompression = fmt.Errorf("unknown compression type")

type UncompressAlg interface {
	Uncompress(input io.Reader) (io.Reader, error)
}

type gzipUncompressAlg struct{}

type deflateUncompressAlg struct{}

type brUncompressAlg struct{}

func New(compressType CompressType) (UncompressAlg, error) {
	switch compressType {
	case Gzip:
		return &gzipUncompressAlg{}, nil
	case Deflate:
		return &deflateUncompressAlg{}, nil
	case BR:
		return &brUncompressAlg{}, nil
	default:
		return nil, fmt.Errorf("%w: %q", ErrUnknownCompression, compressType)
	}
}

func (g *gzipUncompressAlg) Uncompress(input io.Reader) (io.Reader, error) {
	return gzip.NewReader(input)
}

func (d *deflateUncompressAlg) Uncompress(input io.Reader) (io.Reader, error) {
	return flate.NewReader(input), nil
}

func (b *brUncompressAlg) Uncompress(input io.Reader) (io.Reader, error) {
	return bzip2.NewReader(input), nil
}
