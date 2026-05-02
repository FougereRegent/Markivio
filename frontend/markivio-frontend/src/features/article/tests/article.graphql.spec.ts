import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ref } from 'vue'
import type { Article } from '@/features/article/models/article.models'


// Mock urql with gql export
vi.mock('@urql/vue', async () => {
  const actual = await vi.importActual('@urql/vue')
  return {
    ...actual,
    useQuery: vi.fn(() => ({
      data: ref(null),
      error: ref(null),
      fetching: ref(false),
      executeQuery: vi.fn(),
    })),
    useMutation: vi.fn(() => ({
      executeMutation: vi.fn().mockResolvedValue({ data: {} }),
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

// Import after mocks
import {
  useGetArticles,
  useCreateArticle,
  useGetSourceUrl,
  useGetArticleById,
  useUpdateArticle,
} from '@/features/article/composables/article.graphql'

describe('article.graphql composables', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('useGetArticles', () => {
    it('Should initialize with correct structure', () => {
      const offset = ref(0)
      const result = useGetArticles(offset, 10)

      expect(result.articles).toBeDefined()
      expect(result.error).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.hasNext).toBeDefined()
      expect(result.executeQuery).toBeDefined()
    })

    it('Should return computed articles', () => {
      const offset = ref(0)
      const { articles } = useGetArticles(offset, 10)

      // Test that it returns a computed ref
      expect(articles.value).toBeUndefined() // Initially null data
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
})
