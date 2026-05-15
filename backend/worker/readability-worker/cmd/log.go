package main

import (
	"log/slog"
	"os"
)


func initLog() {
	handler := slog.NewJSONHandler(os.Stdout, nil)
	logger := slog.New(handler)

	slog.SetDefault(logger)
}
