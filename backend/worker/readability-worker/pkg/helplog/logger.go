package helplog

import (
	"fmt"
	"log/slog"
)

func PanicIfError(err error, msg string) {
	if err != nil {
		slog.Error(fmt.Sprintf("%s: %s", msg, err))
		panic(err)
	}
}

