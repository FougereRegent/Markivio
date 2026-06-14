package scraping

import (
	"io"
	"strings"

	mapset "github.com/deckarep/golang-set/v2"
	"golang.org/x/net/html"
)

var (
	textElementSet mapset.Set[string] = mapset.NewSet[string]()
)

type htmlScoring struct {
	numberOfScript int
	numberBalise int
	baliseCounter map[string]int
}

type Score struct {
	TextRatio float64
	ScriptRatio float64
}

func init() {
	elements := []string{
		"p", "a", "li", "strong", "pre", "table", "section", "ul", "i", "h1", "h2", "h3", "h4", "h5", "h6", "aside", "em", "thead", "tbody", "td", "th", "b", "mark", "small", "del", "ins", "sub", "sup", "blockquote", "q", "code", "pre", "kbd", "samp", "ol", "dl", "main",
	}
	textElementSet = mapset.NewSet[string]()

	for _, element := range elements {
		textElementSet.Add(element)
	}
}

func scoring(reader io.Reader) (Score, error) {
	node, err := html.Parse(reader)
	if err != nil {
		return Score{}, err
	}

	scoring := htmlScoring{
		baliseCounter: make(map[string]int),
	}
	scoring.parseHtml(node)

	score := Score{
		TextRatio: scoring.calculateRatioText(),
		ScriptRatio: scoring.calculateRatioScript() ,
	}
	return score, nil
}

func (s *htmlScoring) parseHtml(node *html.Node) {
	if node.Type == html.ElementNode && isTextElement(node) {
		s.numberBalise++ 
		v, ok := s.baliseCounter[node.Data]
		if !ok {
			s.baliseCounter[node.Data] = 1
		} else {
			s.baliseCounter[node.Data] = v + 1
		}
	} else if node.Type == html.TextNode {
		return
	} else if node.Type == html.ElementNode && node.Data == "script"{
		s.numberOfScript++
		s.numberBalise++
	} 
	for c := node.FirstChild; c != nil; c = c.NextSibling {
		s.parseHtml(c)
	}
}

func isTextElement(node *html.Node) bool {
	el := strings.ToLower(node.Data)
	return textElementSet.ContainsOne(el)
}

func (s htmlScoring) calculateRatioScript() float64 {
	if s.numberBalise == 0 {
		return 0
	}
	result := float64(s.numberOfScript) / float64(s.numberBalise)
	return result
}

func (s htmlScoring) calculateRatioText() float64 {
	if len(s.baliseCounter) == 0 || s.numberBalise == 0 {
		return 0
	}

	var sum int
	for _, value := range s.baliseCounter {
		sum += value
	}
	result := float64(sum / len(s.baliseCounter)) / float64(s.numberBalise)
	return result
}
