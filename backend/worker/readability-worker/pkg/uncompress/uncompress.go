package uncompress

import (
	"compress/flate"
	"compress/gzip"
	"compress/bzip2"
	"io"
)

type COMPRESS_TYPE string

const (
	GZIP COMPRESS_TYPE = "gzip"
	DEFLATE COMPRESS_TYPE = "deflate"
	BR COMPRESS_TYPE = "br"
)

type UncompressAlg interface {
	Uncompress(input io.Reader) (io.Reader, error)
}

type gzipUncompressAlg struct {}

type deflateUncompressAlg struct {}

type brUncompressAlg struct {}

func New(compressType COMPRESS_TYPE) UncompressAlg {
	switch compressType {
	case GZIP:
		return &gzipUncompressAlg{}
	case DEFLATE:
		return &deflateUncompressAlg{}
	case BR:
		return  &brUncompressAlg{}
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
