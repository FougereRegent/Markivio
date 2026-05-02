import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ref, nextTick } from 'vue'
import { useTagAutocomplete } from '@/features/drawer/composables/tag.drawer.composable'
import type { Tag } from '@/features/tag/models/tag.models'

// ---- MOCKS ----

const tagsMock = ref<Tag[]>([])
const executeQueryMock = vi.fn()

vi.mock('@/features/tag/composables/tag.graphql', () => ({
  useGetAllTags: () => ({
    tags: tagsMock,
    executeQuery: executeQueryMock,
  }),
}))

vi.mock('@vueuse/core', () => ({
  useDebounce: (v: unknown) => ({ value: v }),
}))

vi.mock('@/config/constante.config', () => ({
  CONST: {
    debounceTime: {
      inputTime: 0,
    },
  },
}))

// ---- TESTS ----

describe('useTagAutocomplete', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    tagsMock.value = []
  })

  it('initialise correctement les refs', () => {
    const selectedTags = ref<Tag[]>([])

    const { tagName, refSuggestion } = useTagAutocomplete(selectedTags)

    expect(tagName.value).toBe('')
    expect(refSuggestion.value).toEqual([])
  })

  it('met à jour les suggestions en filtrant les tags déjà sélectionnés', async () => {
    const selectedTags = ref<Tag[]>([{ id: '1', name: 'Vue', color: '' }])

    const { refSuggestion } = useTagAutocomplete(selectedTags)

    tagsMock.value = [
      { id: '1', name: 'Vue', color: '' },
      { id: '2', name: 'React', color: '' },
    ]

    await nextTick()

    expect(refSuggestion.value).toEqual([{ id: '2', name: 'React', color: '' }])
  })

  it('retourne toutes les suggestions si aucun tag sélectionné', async () => {
    const selectedTags = ref<Tag[]>([])

    const { refSuggestion } = useTagAutocomplete(selectedTags)

    tagsMock.value = [
      { id: '1', name: 'Vue', color: '' },
      { id: '2', name: 'React', color: '' },
    ]

    await nextTick()

    expect(refSuggestion.value).toEqual(tagsMock.value)
  })

  it('met refSuggestion à [] si tags est null/undefined', async () => {
    const selectedTags = ref<Tag[]>([])

    const { refSuggestion } = useTagAutocomplete(selectedTags)

    tagsMock.value = []

    await nextTick()

    expect(refSuggestion.value).toEqual([])
  })

  it('search met à jour tagName et reset offset', () => {
    const selectedTags = ref<Tag[]>([])

    const { tagName, search } = useTagAutocomplete(selectedTags)

    search('vue')

    expect(tagName.value).toBe('vue')
  })

  it('search appelle executeQuery si query vide', () => {
    const selectedTags = ref<Tag[]>([])

    const { search } = useTagAutocomplete(selectedTags)

    search('')

    expect(executeQueryMock).toHaveBeenCalledWith({
      requestPolicy: 'network-only',
    })
  })

  it("search N'appelle PAS executeQuery si query non vide", () => {
    const selectedTags = ref<Tag[]>([])

    const { search } = useTagAutocomplete(selectedTags)

    search('vue')

    expect(executeQueryMock).not.toHaveBeenCalled()
  })
})
