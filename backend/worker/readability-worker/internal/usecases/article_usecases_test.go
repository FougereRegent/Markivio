package usescases

import (
	"context"
	"errors"
	"io"
	"strings"
	"testing"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/domain"
	"github.com/google/uuid"
)

type mockReadability struct {
	convertFunc func(url string, ctx context.Context) (io.Reader, error)
}

func (m *mockReadability) ConvertWebSiteToMarkdown(url string, ctx context.Context) (io.Reader, error) {
	return m.convertFunc(url, ctx)
}

type mockArticleRepo struct {
	updateFunc func(ctx context.Context, article *domain.Article) (*domain.Article, error)
}

func (m *mockArticleRepo) UpdateArticleContent(ctx context.Context, article *domain.Article) (*domain.Article, error) {
	return m.updateFunc(ctx, article)
}

func TestHandleReadability_Success(t *testing.T) {
	articleID := uuid.New()
	markdownContent := "# Title\n\nSome content"
	expectedURL := "https://example.com/article"

	readMock := &mockReadability{
		convertFunc: func(url string, ctx context.Context) (io.Reader, error) {
			if url != expectedURL {
				t.Errorf("url = %s, want %s", url, expectedURL)
			}
			return strings.NewReader(markdownContent), nil
		},
	}

	repoMock := &mockArticleRepo{
		updateFunc: func(ctx context.Context, article *domain.Article) (*domain.Article, error) {
			if article.Id != articleID {
				t.Errorf("article.Id = %v, want %v", article.Id, articleID)
			}
			if article.ArticleContent != markdownContent {
				t.Errorf("article.ArticleContent = %s, want %s", article.ArticleContent, markdownContent)
			}
			return article, nil
		},
	}

	uc := NewArticleUseCase(readMock, repoMock)
	evt := domain.CreateReadabilityEvt{
		ArticleId: articleID,
		Url:       expectedURL,
	}

	err := uc.HandleReadability(evt, context.Background())
	if err != nil {
		t.Fatalf("HandleReadability() unexpected error: %v", err)
	}
}

func TestHandleReadability_ConvertError(t *testing.T) {
	expectedErr := errors.New("network error")

	readMock := &mockReadability{
		convertFunc: func(url string, ctx context.Context) (io.Reader, error) {
			return nil, expectedErr
		},
	}

	repoMock := &mockArticleRepo{
		updateFunc: func(ctx context.Context, article *domain.Article) (*domain.Article, error) {
			t.Error("UpdateArticleContent should not be called when Convert fails")
			return nil, nil
		},
	}

	uc := NewArticleUseCase(readMock, repoMock)
	evt := domain.CreateReadabilityEvt{
		ArticleId: uuid.New(),
		Url:       "https://example.com/article",
	}

	err := uc.HandleReadability(evt, context.Background())
	if err == nil {
		t.Fatal("expected error, got nil")
	}
	if !errors.Is(err, expectedErr) {
		t.Errorf("err should wrap %v, got %v", expectedErr, err)
	}
}

func TestHandleReadability_RepoError(t *testing.T) {
	expectedErr := errors.New("database error")

	readMock := &mockReadability{
		convertFunc: func(url string, ctx context.Context) (io.Reader, error) {
			return strings.NewReader("some content"), nil
		},
	}

	repoMock := &mockArticleRepo{
		updateFunc: func(ctx context.Context, article *domain.Article) (*domain.Article, error) {
			return nil, expectedErr
		},
	}

	uc := NewArticleUseCase(readMock, repoMock)
	evt := domain.CreateReadabilityEvt{
		ArticleId: uuid.New(),
		Url:       "https://example.com/article",
	}

	err := uc.HandleReadability(evt, context.Background())
	if err == nil {
		t.Fatal("expected error, got nil")
	}
	if !errors.Is(err, expectedErr) {
		t.Errorf("err should wrap %v, got %v", expectedErr, err)
	}
}
