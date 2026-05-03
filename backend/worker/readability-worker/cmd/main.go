package main

import (
	"fmt"
	"io"
	"log"
	"net/http"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure"
)

func main() {
	testReadInfra := infrastructure.NewReadability(&http.Client{
	})

	result, err := testReadInfra.ConvertWebSiteToMarkdown("https://fr.wikipedia.org/wiki/Informatique")

	if err != nil {
		log.Panicf("%s", err)
	}

	data, err := io.ReadAll(result)
	if err != nil {
		log.Panicf("%s", err)
	}

	fmt.Println(string(data))
}
