import { describe, it, expect, beforeEach, vi } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

const { mockUseGetArticles } = vi.hoisted(() => ({
  mockUseGetArticles: vi.fn(),
}))

vi.mock('@/features/article/composables/article.graphql', () => ({
  useGetArticles: mockUseGetArticles,
}))

import { useArticleStore } from '@/stores/article-store'

describe('article-store', () => {
  beforeEach(async () => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
    const { ref, computed } = await import('vue')
    mockUseGetArticles.mockReturnValue({
      articles: computed(() => []),
      error: ref(null),
      fetching: ref(false),
      hasNext: computed(() => false),
      executeQuery: vi.fn(),
    })
  })

  it('should initialize with offset at 0', () => {
    const store = useArticleStore()
    expect(store.offset).toBe(0)
  })

  it('should call useGetArticles with correct initial parameters', () => {
    useArticleStore()
    expect(mockUseGetArticles).toHaveBeenCalledOnce()
    const [offsetRef, limit] = mockUseGetArticles.mock.calls[0]!
    expect(offsetRef.value).toBe(0)
    expect(limit).toBe(15)
  })

  it('should expose articles from the composable', () => {
    const store = useArticleStore()
    expect(Array.isArray(store.articles)).toBe(true)
  })

  it('should expose hasNext from the composable', () => {
    const store = useArticleStore()
    expect(store.hasNext).toBe(false)
  })

  describe('changeTagNameFilter', () => {
    it('should reset offset to 0', () => {
      const store = useArticleStore()
      store.offset = 10
      store.changeTagNameFilter('vue')
      expect(store.offset).toBe(0)
    })
  })

  describe('changeTypeFilter', () => {
    it('should reset offset to 0', () => {
      const store = useArticleStore()
      store.offset = 10
      store.changeTypeFilter()
      expect(store.offset).toBe(0)
    })
  })

  describe('changeOffset', () => {
    it('should set offset to the given value', () => {
      const store = useArticleStore()
      store.changeOffset(20)
      expect(store.offset).toBe(20)
    })

    it('should accept 0', () => {
      const store = useArticleStore()
      store.offset = 5
      store.changeOffset(0)
      expect(store.offset).toBe(0)
    })
  })

  it('should expose executeQuery as a function', () => {
    const store = useArticleStore()
    expect(store.executeQuery).toBeDefined()
    expect(typeof store.executeQuery).toBe('function')
  })
})
