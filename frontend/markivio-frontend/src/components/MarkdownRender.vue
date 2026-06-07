<script setup lang="ts">
import markdownit from "markdown-it";
import hljs from 'highlight.js';
import 'highlight.js/styles/github.css'; // Style de code (tu peux choisir un autre)
import { computed } from "vue";

const content = defineModel('content', { required: true, type: String });
const md = markdownit({
  html: true,    // permet le HTML
  linkify: true, // transforme URLs en liens
  breaks: true,  // sauts de ligne simples
  typographer: false,
  highlight: function (str: string, lang: string): string {

  if (lang && hljs.getLanguage(lang)) {
      try {
        return '<pre><code class="hljs">' +
               hljs.highlight(str, { language: lang, ignoreIllegals: true }).value +
               '</code></pre>';
      } catch (__) {}
    }

    return '<pre><code class="hljs">' + md.utils.escapeHtml(str) + '</code></pre>';
  }
});

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
