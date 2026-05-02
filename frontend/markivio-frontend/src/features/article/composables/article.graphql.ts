import type { ArticleProps } from '@/features/article/components/ArticleComponent.vue';
import { type Article } from '@/features/article/models/article.models'
import type { Tag } from '@/features/tag/models/tag.models';
import { AddArticles, GetArticleById, GetArticles, GetUrlByArticleId, UpdateArticle } from '@/features/article/queries/article.queries'
import { useClientHandle, useMutation, useQuery } from '@urql/vue'
import { computed, toValue, type Ref } from 'vue'

export type UrlSource = {
  id: string
  source: string
  framable: boolean
}

export function useGetArticles(offset: Ref<number>, limit: number) {
  const { data, error, fetching, executeQuery } = useQuery({
    query: GetArticles,
    variables: computed(() => ({ offset: offset.value, limit: limit })),
  })

  const hasNext = computed(() => data.value?.articles.pageInfo.hasNextPage ?? false)
  const articles = computed(() =>
    data.value?.articles.items.map(
      (pre) =>
        ({
          id: pre.id,
          title: pre.title,
          description: pre.description,
          tags: pre.tags.map((pre) => ({ color: pre.color, label: pre.name })),
        }) as ArticleProps,
    ),
  )

  return { articles, error, fetching, hasNext, executeQuery }
}

export function useCreateArticle(article: Ref<Article>) {
  const { executeMutation, error, data, fetching } = useMutation(AddArticles)

  const createArticle = () => {
    const art = toValue(article)
    return executeMutation({
      input: {
        source: art.source,
        description: art.description,
        title: art.title,
        tags: art.tags.map((pre) => ({ id: pre.id })),
      },
    })
  }

  const id = computed(() => data.value?.article.id)
  return { createArticle, error, id, fetching }
}

export function useGetSourceUrl(id: string) {
  const { client } = useClientHandle()
  const runQuery = async () => {
    const result = await client
      .query(GetUrlByArticleId, {
        id: id,
      })
      .toPromise()

    const article = result.data?.articles.items[0]
    return {
      id: article?.id,
      source: article?.source,
      framable: article?.isFramable,
    } as UrlSource
  }

  return { runQuery }
}

export function useGetArticleById(id: Ref<string | null>, options?: { pause?: Ref<boolean> }) {
  const { data, executeQuery, error } = useQuery({
    query: GetArticleById,
    pause: options?.pause,
    variables: computed(() => {
      return {
        id: id.value
      };
    })
  });

  const article = computed(() => {
    const art = data.value?.articles.items[0];

    return {
      id: art?.id,
      title: art?.title,
      description: art?.description,
      source: art?.source,
      tags: art?.tags.map(src => ({
        id: src.id,
        name: src.name,
        color: src.color
      } as Tag))
    } as Article
  })

  return {article, executeQuery, error}
}

export function useUpdateArticle() {
  const { data, executeMutation, fetching, error} = useMutation(UpdateArticle);

  function updateArticle(article: Ref<Article>) {
    const art = toValue(article);
    return executeMutation({
      input: {
        id: art.id,
        title: art.title,
        description: art.description,
        tags: art.tags.map(src => ({id: src.id}))
      }
    });
  }

  return { data, updateArticle, fetching, error}
}
