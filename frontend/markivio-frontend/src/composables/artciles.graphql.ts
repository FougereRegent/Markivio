import type { ArticleProps } from "@/components/ArticleComponent.vue";
import { GetArticles } from "@/graphql/article.queries";
import { useQuery } from "@urql/vue";
import { computed, type Ref } from "vue";

export function useGetArticles(offset: Ref<number>, limit: number) {
  const { data, error, fetching } = useQuery({
    query: GetArticles,
    variables: computed(() => ({ offset: offset.value, limit: limit }))
  });

  const hasNext = computed(() => data.value?.articles.pageInfo.hasNextPage)
  const articles = computed(() => data.value?.articles.items.map(pre => ({
    id: pre.id,
    title: pre.title,
    description: pre.description,
    tags: [],
  } as ArticleProps)));

  return { articles, error, fetching, hasNext };
}
