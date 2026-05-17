package scraping

import (
	"strings"
	"testing"

	"golang.org/x/net/html"
)

func TestIsTextElement(t *testing.T) {
	tests := []struct {
		name     string
		tag      string
		expected bool
	}{
		{name: "paragraph", tag: "p", expected: true},
		{name: "uppercase P", tag: "P", expected: true},
		{name: "anchor", tag: "a", expected: true},
		{name: "list item", tag: "li", expected: true},
		{name: "heading h1", tag: "h1", expected: true},
		{name: "heading h6", tag: "h6", expected: true},
		{name: "unordered list", tag: "ul", expected: true},
		{name: "div not text element", tag: "div", expected: false},
		{name: "script not text element", tag: "script", expected: false},
		{name: "span not text element", tag: "span", expected: false},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			node := &html.Node{
				Type: html.ElementNode,
				Data: tt.tag,
			}
			result := isTextElement(node)
			if result != tt.expected {
				t.Errorf("isTextElement(%q) = %v, want %v", tt.tag, result, tt.expected)
			}
		})
	}
}

func TestScoring_EmptyHTML(t *testing.T) {
	score, err := scoring(strings.NewReader("<html></html>"))
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	if score.TextRatio != 0 {
		t.Errorf("TextRatio = %f, want 0", score.TextRatio)
	}
	if score.ScriptRatio != 0 {
		t.Errorf("ScriptRatio = %f, want 0", score.ScriptRatio)
	}
}

func TestScoring_OnlyTextElements(t *testing.T) {
	html := `<html><body><p>Hello</p><a>link</a></body></html>`
	score, err := scoring(strings.NewReader(html))
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	// p(1), a(1) → total=2
	// TextRatio: sum=2, len=2, sum/len=1, /total=0.5
	// ScriptRatio: 0/2=0
	if score.TextRatio != 0.5 {
		t.Errorf("TextRatio = %f, want 0.5", score.TextRatio)
	}
	if score.ScriptRatio != 0 {
		t.Errorf("ScriptRatio = %f, want 0", score.ScriptRatio)
	}
}

func TestScoring_OnlyScripts(t *testing.T) {
	html := `<html><head><script>var x=1;</script></head><body><script>alert('hi');</script></body></html>`
	score, err := scoring(strings.NewReader(html))
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	// 2 scripts, 2 total elements
	if score.ScriptRatio != 1.0 {
		t.Errorf("ScriptRatio = %f, want 1.0", score.ScriptRatio)
	}
	if score.TextRatio != 0 {
		t.Errorf("TextRatio = %f, want 0", score.TextRatio)
	}
}

func TestScoring_MixedContent(t *testing.T) {
	html := `<html><body><p>a</p><p>b</p><h1>c</h1><script>x</script></body></html>`
	score, err := scoring(strings.NewReader(html))
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	// p(2), h1(1), script(1) → total=4
	// TextRatio: sum=3, len(baliseCounter)=2, sum/len=1 (int div), /total=0.25
	// ScriptRatio: 1/4=0.25
	if score.TextRatio != 0.25 {
		t.Errorf("TextRatio = %f, want 0.25", score.TextRatio)
	}
	if score.ScriptRatio != 0.25 {
		t.Errorf("ScriptRatio = %f, want 0.25", score.ScriptRatio)
	}
}

func TestScoring_NonTextElements(t *testing.T) {
	html := `<html><body><div></div><span></span><nav></nav></body></html>`
	score, err := scoring(strings.NewReader(html))
	if err != nil {
		t.Fatalf("unexpected error: %v", err)
	}
	// No text elements, no scripts → all ratios 0
	if score.TextRatio != 0 {
		t.Errorf("TextRatio = %f, want 0", score.TextRatio)
	}
	if score.ScriptRatio != 0 {
		t.Errorf("ScriptRatio = %f, want 0", score.ScriptRatio)
	}
}

func TestCalculateRatioScript_ZeroBalise(t *testing.T) {
	s := htmlScoring{}
	ratio := s.calculateRatioScript()
	if ratio != 0 {
		t.Errorf("calculateRatioScript() = %f, want 0", ratio)
	}
}

func TestCalculateRatioScript_WithValues(t *testing.T) {
	s := htmlScoring{
		numberOfScript: 2,
		numberBalise:   10,
	}
	ratio := s.calculateRatioScript()
	if ratio != 0.2 {
		t.Errorf("calculateRatioScript() = %f, want 0.2", ratio)
	}
}

func TestCalculateRatioText_EmptyCounter(t *testing.T) {
	s := htmlScoring{
		baliseCounter: map[string]int{},
		numberBalise:  5,
	}
	ratio := s.calculateRatioText()
	if ratio != 0 {
		t.Errorf("calculateRatioText() = %f, want 0", ratio)
	}
}

func TestCalculateRatioText_ZeroBalise(t *testing.T) {
	s := htmlScoring{
		baliseCounter: map[string]int{"p": 1},
		numberBalise:  0,
	}
	ratio := s.calculateRatioText()
	if ratio != 0 {
		t.Errorf("calculateRatioText() = %f, want 0", ratio)
	}
}

func TestCalculateRatioText_WithValues(t *testing.T) {
	s := htmlScoring{
		baliseCounter: map[string]int{"p": 5, "h1": 3},
		numberBalise:  10,
	}
	ratio := s.calculateRatioText()
	// sum=8, len=2, sum/len=4 (int div), /total=0.4
	if ratio != 0.4 {
		t.Errorf("calculateRatioText() = %f, want 0.4", ratio)
	}
}
