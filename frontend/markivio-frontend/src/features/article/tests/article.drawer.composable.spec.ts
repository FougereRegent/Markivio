import { describe, it, expect, vi, beforeEach } from 'vitest'
import { useArticleForm } from '@/features/drawer/composables/article.drawer.composable'
import type { Tag } from '@/features/tag/models/tag.models'

// mock store
const drawerMock = {
  drawerState: true,
}

vi.mock('@/stores/add-edit-drawer-store', () => ({
  useAddEditDrawer: () => drawerMock,
}))

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

    const tag = { id: "f86ff3bb-d7a1-4be6-828c-a725d0ae8a15", name: 'Vue' } as Tag

    addTag(tag)

    expect(article.value.tags).toContainEqual(tag)
  })

  it('n’ajoute pas un tag en double', () => {
    const { article, addTag } = useArticleForm()

    const tag = { id: "50b28b1b-be47-4314-8b52-65a3083249a0", name: 'Vue' } as Tag

    addTag(tag)
    addTag(tag)

    expect(article.value.tags.length).toBe(1)
  })

  it('supprime un tag', () => {
    const { article, removeTag } = useArticleForm()

    const tag = { id: "4533cf4f-7238-4df4-9eba-5cc093c41397", name: 'Vue' } as Tag
    article.value.tags = [tag]

    removeTag(tag)

    expect(article.value.tags).toEqual([])
  })
})
