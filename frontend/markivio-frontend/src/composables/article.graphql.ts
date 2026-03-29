import type { ArticleProps } from "@/components/ArticleComponent.vue";
import type { Article } from "@/domain/article.models";
import { AddArticles, GetArticles, GetUrlByArticleId } from "@/graphql/article.queries";
import { useClientHandle, useMutation, useQuery } from "@urql/vue";
import { computed, toValue, type Ref } from "vue";


export type UrlSource =  {
  id: string,
  source: string,
  framable: boolean,
};

export function useGetArticles(offset: Ref<number>, limit: number) {
  const { data, error, fetching, executeQuery } = useQuery({
    query: GetArticles,
    variables: computed(() => ({ offset: offset.value, limit: limit }))
  });

  const hasNext = computed(() => data.value?.articles.pageInfo.hasNextPage ?? false)
  const articles = computed(() => data.value?.articles.items.map(pre => ({
    id: pre.id,
    title: pre.title,
    description: pre.description,
    tags: pre.tags.map(pre => ({color: pre.color, label: pre.name})),
  } as ArticleProps)));

  return { articles, error, fetching, hasNext, executeQuery };
}

export function useCreateArticle(article: Ref<Article>) {
  const { executeMutation, error, data, fetching } = useMutation(AddArticles);

  const createArticle = () => {
    const art = toValue(article);
    return executeMutation({
      input: {
        source: art.source,
        description: art.description,
        title: art.title,
        tags: art.tags.map(pre => ({ id: pre.id }))
      }
    });
  };

  const id = computed(() => data.value?.article.id)
  return { createArticle, error, id, fetching }
}

export function useGetSourceUrl(id:string) {
  const { client } = useClientHandle()
  const runQuery = async () => {
    const result = await client.query(GetUrlByArticleId, {
      id: id
    })
    .toPromise();

    const article = result.data?.articles.items[0];
    return {
      id: article?.id,
      source: article?.source,
      framable: article?.isFramable
    } as UrlSource
  }

  return { runQuery }
}
