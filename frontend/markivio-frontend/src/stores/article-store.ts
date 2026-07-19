import { defineStore } from "pinia";
import { ref } from "vue";
import { type ArticleFiltering, ArticleTypeFiltering, useGetArticles } from '@/features/article/composables/article.graphql'


export const useArticleStore = defineStore('articles', () => {
  const articleFiltering = ref<ArticleFiltering>({byTypeName: null, byTagName: null});

  const offset = ref(0);
  const {articles, hasNext, executeQuery} = useGetArticles(offset, 15, articleFiltering);

  function changeTagNameFilter(tagName: string) {
    articleFiltering.value.byTagName = tagName;
    offset.value = 0;
  }

  function changeTypeFilter(typeFilter: ArticleTypeFiltering | null = null) {
    articleFiltering.value.byTagName = null;
    articleFiltering.value.byTypeName = typeFilter;
    offset.value = 0;
  }

  function changeOffset(value: number) {
    offset.value = value;
  }

  return { offset, articles, hasNext, changeTagNameFilter, changeTypeFilter, changeOffset, executeQuery }
});
