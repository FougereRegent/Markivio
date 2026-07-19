import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ref } from 'vue'
import type { Ref } from 'vue'
import type { Article } from '@/features/article/models/article.models'
import {
  GetArticles,
  GetArticlesByTagName,
  GetArticlesByIsFavorite,
  GetArticlesByIsNew,
  GetArticlesByIsReaded,
} from '@/features/article/queries/article.queries'


const { mockExecuteMutation, mockUseQuery } = vi.hoisted(() => ({
  mockExecuteMutation: vi.fn().mockResolvedValue({ data: {} }),
  mockUseQuery: vi.fn(() => ({
    data: ref(null),
    error: ref(null),
    fetching: ref(false),
    executeQuery: vi.fn(),
  })),
}))

vi.mock('@urql/vue', async () => {
  const actual = await vi.importActual('@urql/vue')
  return {
    ...actual,
    useQuery: mockUseQuery,
    useMutation: vi.fn(() => ({
      executeMutation: mockExecuteMutation,
      data: ref(null),
      error: ref(null),
      fetching: ref(false),
    })),
    useClientHandle: vi.fn(() => ({
      client: {
        query: vi.fn(() => ({ toPromise: vi.fn().mockResolvedValue({ data: {} }) })),
      },
    })),
  }
})

import {
  useGetArticles,
  useCreateArticle,
  useGetSourceUrl,
  useGetArticleById,
  useUpdateArticle,
  useToggleFavorite,
  useArticleStats,
  ArticleTypeFiltering,
} from '@/features/article/composables/article.graphql'

describe('article.graphql composables', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('useGetArticles', () => {
    it('Should initialize with correct structure', () => {
      const offset = ref(0)
      const articleFiltering: Ref<{ byTagName?: string | null; byTypeName?: ArticleTypeFiltering | null }> = ref({ byTagName: null, byTypeName: null })
      const result = useGetArticles(offset, 10, articleFiltering)

      expect(result.articles).toBeDefined()
      expect(result.error).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.hasNext).toBeDefined()
      expect(result.executeQuery).toBeDefined()
    })

    it('Should return computed articles', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: null, byTypeName: null })
      const { articles } = useGetArticles(offset, 10, articleFiltering)

      expect(articles.value).toBeUndefined()
    })

    it('Should use GetArticles when no filter is set', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: null, byTypeName: null })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticles)
    })

    it('Should use GetArticles when byTypeName is "all"', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: null, byTypeName: ArticleTypeFiltering.all })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticles)
    })

    it('Should use GetArticlesByIsFavorite when byTypeName is "favorite"', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: null, byTypeName: ArticleTypeFiltering.favorite })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticlesByIsFavorite)
    })

    it('Should use GetArticlesByIsNew when byTypeName is "new"', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: null, byTypeName: ArticleTypeFiltering.new })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticlesByIsNew)
    })

    it('Should use GetArticlesByIsReaded when byTypeName is "archived"', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: null, byTypeName: ArticleTypeFiltering.archived })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticlesByIsReaded)
    })

    it('Should use GetArticlesByTagName when byTagName is set', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: 'vue', byTypeName: null })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticlesByTagName)
    })

    it('Should prioritize byTagName over byTypeName', () => {
      const offset = ref(0)
      const articleFiltering = ref({ byTagName: 'vue', byTypeName: ArticleTypeFiltering.favorite })
      useGetArticles(offset, 10, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query.value).toBe(GetArticlesByTagName)
    })

    it('Should pass correct variables to useQuery', () => {
      const offset = ref(5)
      const articleFiltering = ref({ byTagName: 'react', byTypeName: null })
      useGetArticles(offset, 15, articleFiltering)

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.variables.value).toEqual({
        offset: 5,
        limit: 15,
        articleName: 'react',
      })
    })
  })

  describe('useCreateArticle', () => {
    it('Should initialize with correct structure', () => {
      const article = ref<Article>({
        id: null,
        title: 'New Article',
        source: 'https://example.com',
        description: 'Desc',
        tags: [],
      })

      const result = useCreateArticle(article)

      expect(result.createArticle).toBeDefined()
      expect(result.id).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.error).toBeDefined()
    })
  })

  describe('useGetSourceUrl', () => {
    it('Should return runQuery function', () => {
      const result = useGetSourceUrl('123')

      expect(result.runQuery).toBeDefined()
      expect(typeof result.runQuery).toBe('function')
    })
  })

  describe('useGetArticleById', () => {
    it('Should initialize with correct structure', () => {
      const id = ref('123')
      const result = useGetArticleById(id, { pause: ref(false) })

      expect(result.article).toBeDefined()
      expect(result.executeQuery).toBeDefined()
      expect(result.error).toBeDefined()
    })
  })

  describe('useUpdateArticle', () => {
    it('Should initialize with correct structure', () => {
      const result = useUpdateArticle()

      expect(result.updateArticle).toBeDefined()
      expect(result.data).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.error).toBeDefined()
    })
  })

  describe('useToggleFavorite', () => {
    it('Should initialize with correct structure', () => {
      const result = useToggleFavorite()

      expect(result.toggleFavorite).toBeDefined()
      expect(typeof result.toggleFavorite).toBe('function')
      expect(result.data).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.error).toBeDefined()
    })

    it('Should call executeMutation with correct input', async () => {
      const result = useToggleFavorite()
      const mockId = 'article-123'

      await result.toggleFavorite(mockId)

      expect(mockExecuteMutation).toHaveBeenCalledWith({
        input: mockId,
      })
    })
  })

  describe('useArticleStats', () => {
    it('Should initialize with correct structure', () => {
      const result = useArticleStats()

      expect(result.stats).toBeDefined()
      expect(result.executeQuery).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.error).toBeDefined()
    })

    it('Should call useQuery with GetArticleStatsByCategories', () => {
      useArticleStats()

      const callArgs = mockUseQuery.mock.calls[0][0]
      expect(callArgs.query).toBeDefined()
    })
  })
})
