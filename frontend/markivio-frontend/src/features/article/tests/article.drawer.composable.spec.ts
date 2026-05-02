import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ref } from 'vue'
import type { Tag } from '@/features/tag/models/tag.models'
import type { Article } from '@/features/article/models/article.models'

// mock store
const drawerMock = {
  drawerState: true,
  drawerType: 0, // ActionDrawer.Create
  drawerArticleId: null,
  open: vi.fn(),
  close: vi.fn(),
  drawerTitle: 'Create',
}

vi.mock('@/stores/add-edit-drawer-store', () => ({
  useAddEditDrawer: () => drawerMock,
  ActionDrawer: { Create: 0, Edit: 1 },
}))

// Mock urql
vi.mock('@urql/vue', async () => {
  const actual = await vi.importActual('@urql/vue')
  return {
    ...actual,
    useMutation: () => ({
      executeMutation: vi.fn().mockResolvedValue({ data: {} }),
      fetching: ref(false),
      error: ref(null),
    }),
    useQuery: () => ({
      data: ref(null),
      executeQuery: vi.fn(),
      error: ref(null),
    }),
    useClientHandle: () => ({
      client: {
        query: () => ({ toPromise: vi.fn().mockResolvedValue({ data: {} }) }),
      },
    }),
  }
})

// Mock zod composable
vi.mock('@/features/auth/composables/zod.composable', () => ({
  useZodValidation: () => ({
    validate: vi.fn().mockReturnValue(true),
    errors: ref(null),
  }),
}))

// Import after mocks
import { useArticleForm } from '@/features/drawer/composables/article.drawer.composable'
import { useArticleSubmit } from '@/features/drawer/composables/article.drawer.composable'
import { useArticleDrawer } from '@/features/drawer/composables/article.drawer.composable'
import { ActionDrawer } from '@/stores/add-edit-drawer-store'

describe('useArticleForm', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    drawerMock.drawerState = true
  })

  it('initialise correctement', () => {
    const { article, tagName } = useArticleForm()

    expect(article.value).toEqual({
      id: null,
      title: '',
      source: '',
      description: '',
      tags: [],
    })

    expect(tagName.value).toBe('')
  })

  it('ajoute un tag', () => {
    const { article, addTag } = useArticleForm()

    const tag = { id: 'f86ff3bb-d7a1-4be6-828c-a725d0ae8a15', name: 'Vue' } as Tag

    addTag(tag)

    expect(article.value.tags).toContainEqual(tag)
  })

  it('does not add duplicate tag', () => {
    const { article, addTag } = useArticleForm()

    const tag = { id: '50b28b1b-be47-4314-8b52-65a3083249a0', name: 'Vue' } as Tag

    addTag(tag)
    addTag(tag)

    expect(article.value.tags).toHaveLength(1)
  })

  it('supprime un tag', () => {
    const { article, removeTag } = useArticleForm()

    const tag = { id: '4533cf4f-7238-4df4-9eba-5cc093c41397', name: 'Vue' } as Tag
    article.value.tags = [tag]

    removeTag(tag)

    expect(article.value.tags).toEqual([])
  })
})

describe('useArticleSubmit', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    drawerMock.drawerType = ActionDrawer.Create
  })

  it('should have submit function', () => {
    const article = ref<Article>({
      id: null,
      title: 'Test',
      source: 'https://example.com',
      description: 'Desc',
      tags: [],
    })

    const { submit } = useArticleSubmit(article)

    expect(typeof submit).toBe('function')
  })

  it('should have fetching ref', () => {
    const article = ref<Article>({
      id: null,
      title: 'Test',
      source: 'https://example.com',
      description: 'Desc',
      tags: [],
    })

    const { fetching } = useArticleSubmit(article)

    expect(fetching.value).toBeDefined()
  })

  it('should have hasError computed', () => {
    const article = ref<Article>({
      id: null,
      title: 'Test',
      source: 'https://example.com',
      description: 'Desc',
      tags: [],
    })

    const { hasError } = useArticleSubmit(article)

    expect(hasError.value).toBeDefined()
    expect(hasError.value.title).toBeDefined()
    expect(hasError.value.source).toBeDefined()
  })
})

describe('useArticleDrawer', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should return drawer from setup', () => {
    const article = ref<Article>({
      id: null,
      title: '',
      source: '',
      description: '',
      tags: [],
    })

    const { drawer } = useArticleDrawer(article)

    expect(drawer).toBe(drawerMock)
  })
})
