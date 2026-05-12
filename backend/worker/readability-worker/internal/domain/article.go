package domain

import "github.com/google/uuid"

type Article struct {
	Id uuid.UUID
	Title string
	Description string
}
