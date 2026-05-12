package uncompress

import (
	"compress/bzip2"
	"compress/flate"
	"compress/gzip"
	"io"
)

type CompressType string

const (
	Gzip    CompressType = "gzip"
	Deflate CompressType = "deflate"
	BR      CompressType = "br"
)

type UncompressAlg interface {
	Uncompress(input io.Reader) (io.Reader, error)
}

type gzipUncompressAlg struct{}

type deflateUncompressAlg struct{}

type brUncompressAlg struct{}

func New(compressType CompressType) UncompressAlg {
	switch compressType {
	case Gzip:
		return &gzipUncompressAlg{}
	case Deflate:
		return &deflateUncompressAlg{}
	case BR:
		return &brUncompressAlg{}
	default:
		return nil
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
