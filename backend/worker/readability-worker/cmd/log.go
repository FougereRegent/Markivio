package main

import (
	"log/slog"
	"os"
)


func initLog() *slog.Logger {
	handler := slog.NewJSONHandler(os.Stdout, nil)
	logger := slog.New(handler)

	slog.SetDefault(logger)
	return logger
}
