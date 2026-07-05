import { describe, it, expect, vi, beforeEach } from 'vitest'
import { shallowMount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { nextTick } from 'vue'

const { mockUseGetArticles } = vi.hoisted(() => ({
  mockUseGetArticles: vi.fn(),
}))

vi.mock('@/features/article/composables/article.graphql', () => ({
  useGetArticles: mockUseGetArticles,
}))

vi.mock('@vueuse/core', () => ({
  useInfiniteScroll: vi.fn(() => ({
    reset: vi.fn(),
  })),
}))

import DashBoard from '@/pages/DashBoard.vue'
import ArticleComponent from '@/features/article/components/ArticleComponent.vue'
import { useAddEditDrawer } from '@/stores/add-edit-drawer-store'

describe('DashBoard', () => {
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

  it('should render without crashing', () => {
    const wrapper = shallowMount(DashBoard)
    expect(wrapper.exists()).toBe(true)
  })

  it('should render the articles container', () => {
    const wrapper = shallowMount(DashBoard)
    const container = wrapper.find('div.flex')
    expect(container.exists()).toBe(true)
  })

  it('should render no articles when articlesProps is empty', () => {
    const wrapper = shallowMount(DashBoard)
    const articleStubs = wrapper.findAllComponents(ArticleComponent)
    expect(articleStubs).toHaveLength(0)
  })

  it('should render ArticleComponents when articles arrive', async () => {
    const { ref, computed } = await import('vue')
    const testArticles = computed(() => [
      { id: '1', title: 'Article 1', description: 'Desc 1', tags: [] },
      { id: '2', title: 'Article 2', tags: [{ label: 'vue', color: '#000' }] },
    ])
    mockUseGetArticles.mockReturnValue({
      articles: testArticles,
      error: ref(null),
      fetching: ref(false),
      hasNext: computed(() => false),
      executeQuery: vi.fn(),
    })

    const wrapper = shallowMount(DashBoard)
    await nextTick()

    const articleStubs = wrapper.findAllComponents(ArticleComponent)
    expect(articleStubs).toHaveLength(2)
  })

  it('should pass correct props to ArticleComponent', async () => {
    const { ref, computed } = await import('vue')
    const testArticles = computed(() => [
      { id: '99', title: 'Test Title', description: 'Test Desc', tags: [] },
    ])
    mockUseGetArticles.mockReturnValue({
      articles: testArticles,
      error: ref(null),
      fetching: ref(false),
      hasNext: computed(() => false),
      executeQuery: vi.fn(),
    })

    const wrapper = shallowMount(DashBoard)
    await nextTick()

    const articleStub = wrapper.findComponent(ArticleComponent)
    expect(articleStub.props('id')).toBe('99')
    expect(articleStub.props('title')).toBe('Test Title')
  })

  describe('drawer interaction', () => {
    it('should reset articles and call executeQuery when drawer closes from open state', async () => {
      const { ref, computed } = await import('vue')
      const mockQuery = vi.fn()
      mockUseGetArticles.mockReturnValue({
        articles: computed(() => [{ id: '1', title: 'Test', tags: [] }]),
        error: ref(null),
        fetching: ref(false),
        hasNext: computed(() => false),
        executeQuery: mockQuery,
      })

      const wrapper = shallowMount(DashBoard)
      await nextTick()

      expect(wrapper.findAllComponents(ArticleComponent)).toHaveLength(1)

      const drawer = useAddEditDrawer()
      drawer.open()
      await nextTick()

      expect(wrapper.findAllComponents(ArticleComponent)).toHaveLength(1)

      drawer.close()
      await nextTick()

      expect(wrapper.findAllComponents(ArticleComponent)).toHaveLength(0)
      expect(mockQuery).toHaveBeenCalled()
    })
  })
})
