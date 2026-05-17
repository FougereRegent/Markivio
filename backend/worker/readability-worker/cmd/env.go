package main

type EnvName string

const (
	PgUsernameEnv EnvName = "WORKER_PG_USERNAME"
	PgPasswordEnv EnvName = "WORKER_PG_PASSWORD"
	PgHostEnv     EnvName = "WORKER_PG_HOST"
	PgPortEnv     EnvName = "WORKER_PG_PORT"
	PgDbEnv    EnvName = "WORKER_PG_DB"
	MqUserEnv     EnvName = "WORKER_MQ_USER"
	MqPasswordEnv EnvName = "WORKER_MQ_PASSWORD"
	MqHostEnv     EnvName = "WORKER_MQ_HOST"
	MqPortEnv     EnvName = "WORKER_MQ_PORT"
)

type Config struct {
	PgUsername string
	PgPassword string
	PgPort     string
	PgHost     string
	PgDb       string
	MqUser     string
	MqPassword string
	MqHost     string
	MqPort     string
}

