//go:build container
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
		PgDb: os.Getenv(string(PgDbEnv)),
		MqUser:     os.Getenv(string(MqUserEnv)),
		MqPassword: os.Getenv(string(MqPasswordEnv)),
		MqHost: os.Getenv(string(MqHostEnv)),
		MqPort: os.Getenv(string(MqPortEnv)),
	}
}
