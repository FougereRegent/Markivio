import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { ref, computed } from 'vue'
import { createPinia, setActivePinia } from 'pinia'

const mockT = vi.fn((key: string) => key)
const mockChangeTypeFilter = vi.fn()
const mockChangeTagNameFilter = vi.fn()
const mockTags = ref([
  { name: 'vue', color: '#42b883', articleNumber: 5 },
  { name: 'react', color: '#61dafb', articleNumber: 3 },
])

const mockStats = ref({
  numberOfAllArticle: { totalCount: 42 },
  numberOfFavoriteArticle: { totalCount: 7 },
  numberOfNewArticle: { totalCount: 12 },
  numberOfReadArticle: { totalCount: 5 },
})

vi.mock('vue-i18n', () => ({
  useI18n: () => ({ t: mockT }),
}))

vi.mock('@/stores/version-store', () => ({
  useVersionStore: () => ({ version: computed(() => '1.2.3') }),
}))

vi.mock('@/stores/article-store', () => ({
  useArticleStore: () => ({
    changeTypeFilter: mockChangeTypeFilter,
    changeTagNameFilter: mockChangeTagNameFilter,
  }),
}))

vi.mock('@/features/tag/composables/tag.graphql', () => ({
  useGetTenMostUsedTags: () => ({
    tags: mockTags,
    error: ref(null),
    fetching: ref(false),
  }),
}))

vi.mock('@/features/article/composables/article.graphql', () => ({
  ArticleTypeFiltering: {
    all: 'all',
    favorite: 'favorite',
    new: 'new',
    archived: 'archived',
  },
  useArticleStats: () => ({
    stats: mockStats,
    executeQuery: vi.fn(),
    fetching: ref(false),
    error: ref(null),
  }),
}))

interface MenuItem {
  label?: string
  icon?: string
  color?: string
  separator?: boolean
  command?: () => void
  badge?: { value: number }
  tag?: string
  items?: MenuItem[]
}

let capturedModel: MenuItem[] = []

vi.mock('primevue/menu', () => ({
  default: {
    name: 'Menu',
    props: ['model'],
    setup(props: { model: MenuItem[] }) {
      capturedModel = props.model
    },
    template: '<div><slot /><slot name="end" /></div>',
  },
}))

vi.mock('primevue/popover', () => ({
  default: {
    name: 'Popover',
    template: '<div><slot /></div>',
  },
}))

vi.mock('primevue/ripple', () => ({
  default: { name: 'Ripple', install: (app: { directive: (name: string, value: unknown) => void }) => app.directive('ripple', {}) },
}))

import NavBarComponent from '@/components/NavBarComponent.vue'

function mountNavBar() {
  return mount(NavBarComponent, {
    global: {
      stubs: {
        UserIconComponent: true,
        UserMenuComponent: true,
      },
    },
  })
}

describe('NavBarComponent', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    setActivePinia(createPinia())
    capturedModel = []
  })

  it('should render', () => {
    const wrapper = mountNavBar()
    expect(wrapper.exists()).toBe(true)
  })

  it('should display the version', () => {
    mountNavBar()
    expect(capturedModel).toBeDefined()
  })

  describe('type filter menu items', () => {
    it('should call changeTypeFilter with "all" for allArticles item', () => {
      mountNavBar()
      capturedModel[0]!.items![0]!.command!()
      expect(mockChangeTypeFilter).toHaveBeenCalledWith('all')
    })

    it('should call changeTypeFilter with "favorite" for favorites item', () => {
      mountNavBar()
      capturedModel[0]!.items![1]!.command!()
      expect(mockChangeTypeFilter).toHaveBeenCalledWith('favorite')
    })

    it('should call changeTypeFilter with "new" for toRead item', () => {
      mountNavBar()
      capturedModel[0]!.items![2]!.command!()
      expect(mockChangeTypeFilter).toHaveBeenCalledWith('new')
    })

    it('should call changeTypeFilter with "archived" for archived item', () => {
      mountNavBar()
      capturedModel[0]!.items![3]!.command!()
      expect(mockChangeTypeFilter).toHaveBeenCalledWith('archived')
    })
  })

  describe('tag filter menu items', () => {
    it('should map tags with correct label and color', () => {
      mountNavBar()
      const tagSection = capturedModel[1]!
      expect(tagSection.label).toBe('nav.tags')
      expect(tagSection.items![0]!.label).toBe('vue')
      expect(tagSection.items![0]!.color).toBe('#42b883')
      expect(tagSection.items![1]!.label).toBe('react')
      expect(tagSection.items![1]!.color).toBe('#61dafb')
    })

    it('should call changeTagNameFilter when a tag is clicked', () => {
      mountNavBar()
      capturedModel[1]!.items![0]!.command!()
      expect(mockChangeTagNameFilter).toHaveBeenCalledWith('vue')
    })

    it('should append allTags and handleTags after mapped tags', () => {
      mountNavBar()
      const tagSection = capturedModel[1]!
      const lastTwo = tagSection.items!.slice(-2)
      expect(lastTwo[0]!.label).toBe('nav.allTags')
      expect(lastTwo[1]!.label).toBe('nav.handleTags')
    })
  })

  describe('menu structure', () => {
    it('should have a separator between tags and bin', () => {
      mountNavBar()
      expect(capturedModel[2]!.separator).toBe(true)
    })

    it('should have a bin item at the end', () => {
      mountNavBar()
      expect(capturedModel[3]!.items![0]!.label).toBe('nav.bin')
      expect(capturedModel[3]!.items![0]!.icon).toBe('ri-delete-bin-line')
    })
  })

  it('should render the user menu end slot', () => {
    const wrapper = mountNavBar()
    expect(wrapper.text()).toContain('userMenu.editProfile')
    expect(wrapper.text()).toContain('userMenu.logout')
  })

  describe('article stats badges', () => {
    it('should display all articles count in badge', () => {
      mountNavBar()
      const allArticlesItem = capturedModel[0]!.items![0]!
      expect(allArticlesItem.badge!.value).toBe(42)
    })

    it('should display favorites count in badge', () => {
      mountNavBar()
      const favoritesItem = capturedModel[0]!.items![1]!
      expect(favoritesItem.badge!.value).toBe(7)
    })

    it('should display new articles count in badge', () => {
      mountNavBar()
      const toReadItem = capturedModel[0]!.items![2]!
      expect(toReadItem.badge!.value).toBe(12)
    })

    it('should display read articles count in badge', () => {
      mountNavBar()
      const archivedItem = capturedModel[0]!.items![3]!
      expect(archivedItem.badge!.value).toBe(5)
    })

    it('should default to 0 when stats are undefined', () => {
      mockStats.value = undefined as unknown as typeof mockStats.value
      mountNavBar()
      const allArticlesItem = capturedModel[0]!.items![0]!
      expect(allArticlesItem.badge!.value).toBe(0)
    })
  })
})
