package main

import (
	"fmt"
	"io"
	"log"
	"net/http"
	"os"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/infrastructure"
)
type EnvName string
const (
	PgUsernameEnv EnvName = "WORKER_PG_USERNAME"
	PgPasswordEnv EnvName = "WORKER_PG_PASSWORD"
	MqUserEnv EnvName = "WORKER_MQ_USER"
	MqPasswordEnv EnvName = "WORKER_MQ_PASSWORD"
)

type Config struct {
	PgUsername string
	PgPassword string
	MqUser string
	MqPassword string
}

var config Config

func init() {
	config = Config{
		PgUsername: os.Getenv(string(PgUsernameEnv)),
		PgPassword: os.Getenv(string(PgPasswordEnv)),
		MqUser: os.Getenv(string(MqUserEnv)),
		MqPassword: os.Getenv(string(MqPasswordEnv)),
	}
}

func main() {
	testReadInfra := infrastructure.NewReadability(&http.Client{
	})

	result, err := testReadInfra.ConvertWebSiteToMarkdown(os.Args[1])

	if err != nil {
		log.Panicf("%s", err)
	}

	data, err := io.ReadAll(result)
	if err != nil {
		log.Panicf("%s", err)
	}

	fmt.Println(string(data))
}
