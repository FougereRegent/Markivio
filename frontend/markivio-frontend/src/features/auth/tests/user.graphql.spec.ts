import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ref } from 'vue'
import type { UserInformation } from '@/features/auth/models/user.models'

// Mock urql with gql export
vi.mock('@urql/vue', async () => {
  const actual = await vi.importActual('@urql/vue')
  return {
    ...actual,
    useQuery: vi.fn(() => ({
      data: ref({ me: { id: '123', email: 'test@example.com', firstName: 'John', lastName: 'Doe' } }),
      error: ref(null),
      fetching: ref(false),
    })),
    useMutation: vi.fn(() => ({
      executeMutation: vi.fn().mockResolvedValue({ data: {} }),
      fetching: ref(false),
      error: ref(null),
    })),
  }
})

// Import after mocks
import { useGetMyUser, useUpdateUserInformation } from '@/features/auth/composables/user.graphql'

describe('user.graphql composables', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('useGetMyUser', () => {
    it('Should initialize with correct structure', () => {
      const result = useGetMyUser()

      expect(result.userInfo).toBeDefined()
      expect(result.error).toBeDefined()
      expect(result.fetching).toBeDefined()
    })

    it('Should return computed user info', () => {
      const { userInfo } = useGetMyUser()

      expect(userInfo.value).toBeDefined()
      expect(userInfo.value?.email).toBe('test@example.com')
    })
  })

  describe('useUpdateUserInformation', () => {
    it('Should initialize with correct structure', () => {
      const user = ref<UserInformation>({
        id: '123',
        email: 'test@example.com',
        firstName: 'John',
        lastName: 'Doe',
      })

      const result = useUpdateUserInformation(user)

      expect(result.updateUser).toBeDefined()
      expect(result.fetching).toBeDefined()
      expect(result.error).toBeDefined()
    })

    it('Should have updateUser as a function', () => {
      const user = ref<UserInformation>({
        id: '123',
        email: 'test@example.com',
        firstName: 'John',
        lastName: 'Doe',
      })

      const { updateUser } = useUpdateUserInformation(user)

      expect(typeof updateUser).toBe('function')
    })
  })
})
