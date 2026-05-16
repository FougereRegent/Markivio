package domain

import "github.com/google/uuid"

type CreateReadabilityEvt struct {
	ArticleId uuid.UUID `json:"id"`
	Url string `json:"url"`
}
