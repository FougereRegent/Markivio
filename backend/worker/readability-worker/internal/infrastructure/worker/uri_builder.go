package worker

import (
	"fmt"
)

type RabbitMqConn struct {
	User string
	Password string
	Host string
	Port string
}

func UriBuilder(info RabbitMqConn) string {
	brokerUri := fmt.Sprintf("amqp://%s:%s@%s:%s", 
		info.User,
		info.Password,
		info.Host,
		info.Port)
	return brokerUri
}
