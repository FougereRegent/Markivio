import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ref } from 'vue'
import type { Tag } from '@/features/tag/models/tag.models'

// Mock urql with gql export
vi.mock('@urql/vue', async () => {
  const actual = await vi.importActual('@urql/vue')
  return {
    ...actual,
    useQuery: vi.fn(() => ({
      data: ref(null),
      fetching: ref(false),
      error: ref(null),
      executeQuery: vi.fn(),
    })),
    useMutation: vi.fn(() => ({
      executeMutation: vi.fn().mockResolvedValue({ data: {} }),
      data: ref(null),
      error: ref(null),
      fetching: ref(false),
      tags: ref(null),
    })),
  }
})

// Import after mocks
import { useGetAllTags, useCreateTags } from '@/features/tag/composables/tag.graphql'

describe('tag.graphql composables', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('useGetAllTags', () => {
    it('Should initialize with correct structure', () => {
      const tagName = ref<string | null>(null)
      const result = useGetAllTags(tagName)

      expect(result.tags).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.error).toBeDefined()
      expect(result.executeQuery).toBeDefined()
    })

    it('Should return undefined when data is null', () => {
      const tagName = ref<string | null>(null)
      const { tags } = useGetAllTags(tagName)

      expect(tags.value).toBeUndefined()
    })
  })

  describe('useCreateTags', () => {
    it('Should initialize with correct structure for single tag', () => {
      const tag = ref<Tag>({
        id: null,
        name: 'New Tag',
        color: '#00ff00',
      })

      const result = useCreateTags(tag)

      expect(result.createTags).toBeDefined()
      expect(result.tags).toBeDefined()
      expect(result.error).toBeDefined()
      expect(result.fetching).toBeDefined()
    })

    it('Should initialize with correct structure for array of tags', () => {
      const tagsInput = ref<Tag[]>([
        { id: null, name: 'Tag1', color: '#ff0000' },
        { id: null, name: 'Tag2', color: '#00ff00' },
      ])

      const result = useCreateTags(tagsInput as any)

      expect(result.createTags).toBeDefined()
      expect(result.tags).toBeDefined()
    })

    it('Should have createTags as a function', () => {
      const tag = ref<Tag>({
        id: null,
        name: 'Test',
        color: '#000',
      })

      const { createTags } = useCreateTags(tag)

      expect(typeof createTags).toBe('function')
    })
  })
})
