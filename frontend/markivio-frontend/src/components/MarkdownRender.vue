<script setup lang="ts">
import 'highlight.js/styles/github.css'; // Style de code (tu peux choisir un autre)
import { computed } from "vue";
import markdownit from "markdown-it";
import markdownItAnchor from 'markdown-it-anchor'
import markdownItAttrs from 'markdown-it-attrs'
import markdownItContainer from 'markdown-it-container'
import markdownItFootnote from 'markdown-it-footnote'
import markdownItToc from 'markdown-it-toc-done-right'
import hljs from 'highlight.js';
import 'github-markdown-css/github-markdown.css'
import 'highlight.js/styles/github-dark.css'

const content = defineModel('content', { required: true, type: String });
const md = markdownit({
  html: true,    // permet le HTML
  linkify: true, // transforme URLs en liens
  breaks: true,  // sauts de ligne simples
  typographer: false,
  highlight: function (str: string, lang: string): string {

    if (lang && hljs.getLanguage(lang)) {
      try {
        return '<pre><code>' +
          hljs.highlight(str, { language: lang, ignoreIllegals: true }).value +
          '</code></pre>';
      } catch { }
    }

    return '<pre><code>' + md.utils.escapeHtml(str) + '</code></pre>';
  }
});

md.use(markdownItAnchor)
  .use(markdownItAttrs)
  .use(markdownItContainer)
  .use(markdownItFootnote)
  .use(markdownItToc)

const renderMarkdown = computed(() => md.render(content.value));

</script>

<template>
  <div v-html="renderMarkdown" class="markdown-body"></div>
</template>

<style>
.markdown-body {
  font-family: sans-serif;
  color: black;
  line-height: 1.6;
}

/* Optional: pour que les tableaux et le code aient un peu de style */
.markdown-body table {
  border-collapse: collapse;
  width: 90%;
}

.markdown-body table,
.markdown-body th,
.markdown-body td {
  border: 1px solid #ccc;
}

.markdown-body th,
.markdown-body td {
  padding: 8px;
  text-align: left;
}
</style>
