//go:build dev
package main

import (
	"os"

	"github.com/joho/godotenv"
)

func init() {
	err := godotenv.Load(".env")
	if err != nil {
		panic("Could not load .env file")
	}

	config = Config{
		PgUsername: os.Getenv(string(PgUsernameEnv)),
		PgPassword: os.Getenv(string(PgPasswordEnv)),
		PgHost: os.Getenv(string(PgHostEnv)),
		PgPort: os.Getenv(string(PgPortEnv)),
		MqUser:     os.Getenv(string(MqUserEnv)),
		MqPassword: os.Getenv(string(MqPasswordEnv)),
	}
}
