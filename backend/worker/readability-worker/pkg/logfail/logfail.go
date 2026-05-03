package logfail

import "log"

func LogPanic(err error, msg string) {
	if err != nil {
		log.Panicf("%s: %s", msg, err)
	}
}

