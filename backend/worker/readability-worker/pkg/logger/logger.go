package logger

import "log"

func PanicIfError(err error, msg string) {
	if err != nil {
		log.Panicf("%s: %s", msg, err)
	}
}

