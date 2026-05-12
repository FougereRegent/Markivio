//go:build prod
package main


import (
	"os"
)
func init() {
	config = Config{
		PgUsername: os.Getenv(string(PgUsernameEnv)),
		PgPassword: os.Getenv(string(PgPasswordEnv)),
		PgHost: os.Getenv(string(PgHostEnv)),
		PgPort: os.Getenv(string(PgPortEnv)),
		MqUser:     os.Getenv(string(MqUserEnv)),
		MqPassword: os.Getenv(string(MqPasswordEnv)),
	}
}
