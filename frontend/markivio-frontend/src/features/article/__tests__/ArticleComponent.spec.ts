import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import ArticleComponent from '@/features/article/components/ArticleComponent.vue'
import type { ArticleProps } from '@/features/article/components/ArticleComponent.vue'

// Mock urql client
vi.mock('@urql/vue', async () => {
  const actual = await vi.importActual('@urql/vue')
  return {
    ...actual,
    useClientHandle: () => ({
      client: {
        query: () => ({ toPromise: vi.fn().mockResolvedValue({ data: {} }) }),
      },
    }),
    useQuery: vi.fn(() => ({
      data: vi.fn(),
      executeQuery: vi.fn(),
      error: vi.fn(),
    })),
  }
})

// Mock store
vi.mock('@/stores/add-edit-drawer-store', () => ({
  useAddEditDrawer: () => ({
    open: vi.fn(),
    close: vi.fn(),
    drawerState: false,
    drawerType: 0,
  }),
}))

describe('ArticleComponent', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  const mountComponent = (props: Partial<ArticleProps> = {}) => {
    const defaultProps: ArticleProps = {
      id: '123',
      title: 'Test Article',
      description: 'A test description',
      tags: [],
      ...props,
    }

    return mount(ArticleComponent, {
      props: defaultProps,
      global: {
        stubs: {
          DialogSource: true,
          Tag: true,
        },
        provide: {
          // Provide urql client if needed
        },
      },
    })
  }

  it('should have correct structure', () => {
    const wrapper = mountComponent()

    expect(wrapper.exists()).toBe(true)
  })

  it('should have container with correct classes', () => {
    const wrapper = mountComponent()

    const container = wrapper.find('div.flex.flex-row')
    expect(container.exists()).toBe(true)
  })

  it('should handle props correctly', () => {
    const wrapper = mountComponent({ title: 'My Article' })

    expect(wrapper.props('title')).toBe('My Article')
  })

  it('should handle description prop', () => {
    const wrapper = mountComponent({
      description: 'This is a test article description',
    })

    expect(wrapper.props('description')).toBe('This is a test article description')
  })

  it('should handle tags prop', () => {
    const wrapper = mountComponent({
      tags: [
        { label: 'Vue', color: '#ff0000' },
      ],
    })

    expect(wrapper.props('tags')).toHaveLength(1)
  })

  it('should handle undefined description', () => {
    const wrapper = mountComponent({ description: undefined })

    expect(wrapper.props('description')).toBeUndefined()
  })

  it('should handle empty tags array', () => {
    const wrapper = mountComponent({ tags: [] })

    expect(wrapper.props('tags')).toHaveLength(0)
  })
})
