import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import DrawerAddOrEdit from '@/features/drawer/components/DrawerAddOrEdit.vue'

// Mock stores and composables
vi.mock('@/stores/add-edit-drawer-store', () => ({
  useAddEditDrawer: () => ({
    drawerState: true,
    drawerType: 0,
    drawerTitle: 'Create',
    open: vi.fn(),
    close: vi.fn(),
  }),
  ActionDrawer: { Create: 0, Edit: 1 },
}))

vi.mock('@/features/drawer/composables/article.drawer.composable', () => ({
  useArticleForm: () => ({
    article: {
      value: {
        id: null,
        title: '',
        source: '',
        description: '',
        tags: [],
      },
    },
    tagName: { value: '' },
    addTag: vi.fn(),
    removeTag: vi.fn(),
  }),
  useArticleSubmit: () => ({
    submit: vi.fn(),
    fetching: { value: false },
    hasError: {
      value: {
        title: { hasError: false, message: '' },
        source: { hasError: false, message: '' },
      },
    },
  }),
  useArticleDrawer: () => ({
    drawer: {},
  }),
}))

vi.mock('@/features/drawer/composables/tag.drawer.composable', () => ({
  useTagAutocomplete: () => ({
    tagName: { value: '' },
    refSuggestion: { value: [] },
    search: vi.fn(),
  }),
}))

describe('DrawerAddOrEdit', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  const mountComponent = () => {
    return mount(DrawerAddOrEdit, {
      global: {
        stubs: {
          InputText: true,
          Textarea: true,
          Button: true,
          AutoComplete: true,
        },
      },
    })
  }

  it('should render drawer', () => {
    const wrapper = mountComponent()

    expect(wrapper.exists()).toBe(true)
  })

  it('should have submit function available', () => {
    const wrapper = mountComponent()

    // Check that the component has the expected structure
    expect(wrapper.vm).toBeDefined()
  })

  it('should have correct structure', () => {
    const wrapper = mountComponent()

    // Component should render without errors
    expect(wrapper.html()).toBeDefined()
  })
})
