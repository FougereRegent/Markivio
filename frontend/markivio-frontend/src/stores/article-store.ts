import { defineStore } from "pinia";
import { ref } from "vue";
import { useGetArticles } from '@/features/article/composables/article.graphql'


export const useArticleStore = defineStore('articles', () => {
  const tagsFilter = ref<string | null>(null);
  const typeFilter = ref<string>("all");

  const offset = ref(0);
  const {articles, hasNext, executeQuery} = useGetArticles(offset, 15, tagsFilter);


  function changeTagNameFilter(tagName: string) {
    tagsFilter.value = tagName;
    offset.value = 0;
  }

  function changeTypeFilter() {
    tagsFilter.value = null;
    typeFilter.value = "";
    offset.value = 0;
  }

  function changeOffset(value: number) {
    offset.value = value;
  }

  return { offset, articles, hasNext, changeTagNameFilter, changeTypeFilter, changeOffset, executeQuery }
});
