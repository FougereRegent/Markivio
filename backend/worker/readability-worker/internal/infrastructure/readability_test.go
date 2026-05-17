package infrastructure

import (
	"crypto/sha256"
	"fmt"
	"io"
	"net/http"
	"testing"

	"github.com/gkampitakis/go-snaps/snaps"
)


func TestReadability(t *testing.T) {
	//Arrange
	urls := []struct{
		Name string
		Url string
	}{
		{
			Name: "Wikipedia_Informatique",
			Url: "https://fr.wikipedia.org/wiki/Informatique",
		},
		{
			Name: "Wikipedia_Math",
			Url: "https://fr.wikipedia.org/wiki/Math_(logiciel)",
		},
		{
			Name: "Golang_Sha256",
			Url: "https://pkg.go.dev/crypto/sha256",
		},
		{
			Name: "Testify",
			Url: "https://github.com/stretchr/testify#installation",
		},
	}
	w := ReadabilityScraper{
		client: &http.Client{},
	}
	//Act & Arrange
	for _, url := range urls {
		name := fmt.Sprintf("TestReadability_%s", url.Name)
		t.Run(name, func(te *testing.T) {
			hash := sha256.New()
			result, err := w.ConvertWebSiteToMarkdown(url.Url, nil)
			if err != nil {
				t.Fail()
			}

			if result == nil {
				t.Fail()
			}
			b, _ := io.ReadAll(result)
			hash.Write(b)
			snaps.MatchSnapshot(t, fmt.Sprintf("%x", hash.Sum(nil)))
		})
	}
}
