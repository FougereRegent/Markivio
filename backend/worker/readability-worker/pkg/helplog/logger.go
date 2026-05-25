package helplog

import (
	"fmt"
	"log/slog"
	"os"
)

func FatalIfError(err error, msg string) {
	if err != nil {
		slog.Error(fmt.Sprintf("%s: %s", msg, err))
		os.Exit(1)
	}
}
