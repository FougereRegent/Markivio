package worker

import "testing"

func TestUriBuilder_Success(t *testing.T) {
	info := RabbitMqConn{
		User:     "guest",
		Password: "guest",
		Host:     "localhost",
		Port:     "5672",
	}

	got := UriBuilder(info)
	want := "amqp://guest:guest@localhost:5672"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}

func TestUriBuilder_SpecialCharacters(t *testing.T) {
	info := RabbitMqConn{
		User:     "admin",
		Password: "p@ss:w0rd!",
		Host:     "rabbitmq.example.com",
		Port:     "5672",
	}

	got := UriBuilder(info)
	want := "amqp://admin:p@ss:w0rd!@rabbitmq.example.com:5672"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}

func TestUriBuilder_EmptyCredentials(t *testing.T) {
	info := RabbitMqConn{
		User:     "",
		Password: "",
		Host:     "localhost",
		Port:     "5672",
	}

	got := UriBuilder(info)
	want := "amqp://:@localhost:5672"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}

func TestUriBuilder_EmptyHost(t *testing.T) {
	info := RabbitMqConn{
		User:     "guest",
		Password: "guest",
		Host:     "",
		Port:     "5672",
	}

	got := UriBuilder(info)
	want := "amqp://guest:guest@:5672"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}

func TestUriBuilder_EmptyPort(t *testing.T) {
	info := RabbitMqConn{
		User:     "guest",
		Password: "guest",
		Host:     "localhost",
		Port:     "",
	}

	got := UriBuilder(info)
	want := "amqp://guest:guest@localhost:"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}

func TestUriBuilder_AllEmpty(t *testing.T) {
	info := RabbitMqConn{
		User:     "",
		Password: "",
		Host:     "",
		Port:     "",
	}

	got := UriBuilder(info)
	want := "amqp://:@:"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}

func TestUriBuilder_DifferentPort(t *testing.T) {
	info := RabbitMqConn{
		User:     "guest",
		Password: "guest",
		Host:     "localhost",
		Port:     "15672",
	}

	got := UriBuilder(info)
	want := "amqp://guest:guest@localhost:15672"

	if got != want {
		t.Fatalf("expected %q, got %q", want, got)
	}
}
