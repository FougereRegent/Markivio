import type { ArticleProps } from '@/features/article/components/ArticleComponent.vue';
import { type Article } from '@/features/article/models/article.models'
import type { Tag } from '@/features/tag/models/tag.models';
import { AddArticles, GetArticleById, GetArticles, GetArticlesByTagName, GetUrlAndContentByArticleId, UpdateArticle, ToggleFavorite, GetArticlesByIsFavorite, GetArticlesByIsNew, GetArticlesByIsReaded, GetArticleStatsByCategories } from '@/features/article/queries/article.queries'
import { useClientHandle, useMutation, useQuery } from '@urql/vue'
import { computed, toValue, type Ref } from 'vue'

export enum ArticleTypeFiltering {
  all = "all" ,
  favorite =  "favorite",
  new = "new",
  archived = "archived"
};

export type UrlSource = {
  id: string
  source: string
  framable: boolean
  content: string
}

export type ArticleFiltering = {
  byTagName?: string | null,
  byTypeName?: ArticleTypeFiltering | null
};

export function useGetArticles(offset: Ref<number>, limit: number, articleFiltering: Ref<ArticleFiltering>) {
  const query = computed(() => {
    if(!articleFiltering.value.byTagName && !articleFiltering.value.byTypeName) {
      return  GetArticles;
    }
    if(articleFiltering.value.byTagName) {
      return GetArticlesByTagName;
    }

    switch(articleFiltering.value.byTypeName)
    {
      case ArticleTypeFiltering.all:
        return GetArticles;
      case ArticleTypeFiltering.favorite:
        return GetArticlesByIsFavorite;
      case ArticleTypeFiltering.new:
        return GetArticlesByIsNew;
      case ArticleTypeFiltering.archived:
        return GetArticlesByIsReaded;
      default:
        return GetArticles;
    }

  });
  const { data, error, fetching, executeQuery } = useQuery({
    query: query,
    variables: computed(() => ({ offset: offset.value, limit: limit, articleName: articleFiltering?.value.byTagName }))  })

  const hasNext = computed(() => data.value?.articles.pageInfo.hasNextPage ?? false)
  const articles = computed(() =>
    data.value?.articles.items.map(
      (pre) =>
        ({
          id: pre.id,
          title: pre.title,
          description: pre.description,
          isFavorite: pre.isFavorite,
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
      .query(GetUrlAndContentByArticleId, {
        id: id,
      })
      .toPromise()

    const article = result.data?.articles.items[0]
    return {
      id: article?.id,
      source: article?.source,
      framable: article?.isFramable,
      content: article?.content
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

  return { article, executeQuery, error }
}

export function useUpdateArticle() {
  const { data, executeMutation, fetching, error } = useMutation(UpdateArticle);

  function updateArticle(article: Ref<Article>) {
    const art = toValue(article);
    return executeMutation({
      input: {
        id: art.id,
        title: art.title,
        description: art.description,
        tags: art.tags.map(src => ({ id: src.id }))
      }
    });
  }

  return { data, updateArticle, fetching, error }
}

export function useToggleFavorite() {
  const { data, executeMutation, fetching, error } = useMutation(ToggleFavorite);

  function toggleFavorite(id: string) {
    return executeMutation({ input: id });
  }

  return { data, toggleFavorite, fetching, error }
}

export function useArticleStats() {
  const { data, executeQuery, fetching, error } = useQuery({
    query: GetArticleStatsByCategories
  });

  return {stats: data, executeQuery, fetching, error}
}
