package scraping

import (
	"testing"

	"github.com/gkampitakis/go-snaps/snaps"
)

func TestCommonHeaders(t *testing.T) {
	header := commonHeaders
	snaps.MatchSnapshot(t, header)
}
