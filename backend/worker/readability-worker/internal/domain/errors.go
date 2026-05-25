package domain

import (
	"errors"
	"fmt"
)

type ErrorCode string

const (
	ErrCodeInvalidURL        ErrorCode = "INVALID_URL"
	ErrCodeArticleNotFound   ErrorCode = "ARTICLE_NOT_FOUND"
	ErrCodeScrapingFailed    ErrorCode = "SCRAPING_FAILED"
	ErrCodeInvalidMessage    ErrorCode = "INVALID_MESSAGE"
	ErrCodeCompressionUnkown ErrorCode = "COMPRESSION_UNKNOWN"
	ErrCodeHTTPStatus        ErrorCode = "HTTP_STATUS_ERROR"
	ErrCodeDatabase          ErrorCode = "DATABASE_ERROR"
	ErrCodeInternal          ErrorCode = "INTERNAL_ERROR"
)

var (
	ErrInvalidURL        = errors.New("invalid URL")
	ErrArticleNotFound   = errors.New("article not found")
	ErrScrapingFailed    = errors.New("scraping failed")
	ErrInvalidMessage    = errors.New("invalid message")
	ErrCompressionUnkown = errors.New("unknown compression type")
)

type DomainError struct {
	Code    ErrorCode
	Message string
	Err     error
}

func (e *DomainError) Error() string {
	if e.Err != nil {
		return fmt.Sprintf("[%s] %s: %v", e.Code, e.Message, e.Err)
	}
	return fmt.Sprintf("[%s] %s", e.Code, e.Message)
}

func (e *DomainError) Unwrap() error {
	return e.Err
}

func NewDomainError(code ErrorCode, msg string, err error) *DomainError {
	return &DomainError{Code: code, Message: msg, Err: err}
}

func NewInvalidURLError(url string, err error) *DomainError {
	return NewDomainError(ErrCodeInvalidURL, fmt.Sprintf("invalid URL: %s", url), err)
}

func NewArticleNotFoundError(id interface{}) *DomainError {
	return NewDomainError(ErrCodeArticleNotFound, fmt.Sprintf("article not found: %v", id), ErrArticleNotFound)
}

func NewScrapingFailedError(url string, err error) *DomainError {
	return NewDomainError(ErrCodeScrapingFailed, fmt.Sprintf("scraping failed for URL: %s", url), err)
}

func NewInvalidMessageError(err error) *DomainError {
	return NewDomainError(ErrCodeInvalidMessage, "invalid message received", err)
}

func NewHTTPStatusError(url string, statusCode int) *DomainError {
	return NewDomainError(ErrCodeHTTPStatus, fmt.Sprintf("HTTP %d for URL: %s", statusCode, url), fmt.Errorf("unexpected HTTP status: %d", statusCode))
}

func NewDatabaseError(msg string, err error) *DomainError {
	return NewDomainError(ErrCodeDatabase, msg, err)
}

func NewInternalError(msg string, err error) *DomainError {
	return NewDomainError(ErrCodeInternal, msg, err)
}
