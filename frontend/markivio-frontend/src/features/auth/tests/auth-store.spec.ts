import { describe, it, expect, vi, beforeEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import type { UserAuth } from '@/features/auth/stores/auth-store'

// Mock Auth0
const mockAuth0 = {
  isAuthenticated: { value: false },
  user: { value: null },
  isLoading: { value: false },
  getAccessTokenSilently: vi.fn(),
  loginWithRedirect: vi.fn(),
  logout: vi.fn(),
}

vi.mock('@auth0/auth0-vue', () => ({
  useAuth0: () => mockAuth0,
}))

// Import after mocks
import { useAuthStore } from '@/features/auth/stores/auth-store'

describe('useAuthStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()

    mockAuth0.isAuthenticated.value = false
    mockAuth0.user.value = null
    mockAuth0.isLoading.value = false
    mockAuth0.getAccessTokenSilently.mockReset()
    mockAuth0.loginWithRedirect.mockReset()
    mockAuth0.logout.mockReset()
  })

  it('Should initialize with empty token', () => {
    const store = useAuthStore()
    expect(store.token).toBe('')
  })

  it('Should set token on init', async () => {
    const store = useAuthStore()
    mockAuth0.getAccessTokenSilently.mockResolvedValue('test-token')

    await store.init()

    expect(store.token).toBe('test-token')
    expect(mockAuth0.getAccessTokenSilently).toHaveBeenCalled()
  })

  it('Should reflect auth0 isAuthenticated state', () => {
    const store = useAuthStore()
    mockAuth0.isAuthenticated.value = true

    expect(store.isAuthenticated).toBe(true)
  })

  it('Should reflect auth0 isLoading state', () => {
    const store = useAuthStore()
    mockAuth0.isLoading.value = true

    expect(store.isLoading).toBe(true)
  })

  it('Should return user from auth0', () => {
    const store = useAuthStore()
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    mockAuth0.user.value = { sub: '123', name: 'John' } as any

    expect(store.user).toBe(mockAuth0.user.value)
  })

it('Should compute getUser correctly', () => {
    const store = useAuthStore()

     
    mockAuth0.user.value = {
      sub: 'auth0|123',
      name: 'John Doe',
      family_name: 'Doe',
picture: 'https://example.com/pic.jpg',
      } as any // eslint-disable-line @typescript-eslint/no-explicit-any

    const userAuth: UserAuth = store.getUser

    expect(userAuth).toEqual({
      authId: 'auth0|123',
      firstName: 'John Doe',
      lastName: 'Doe',
      accountPicture: 'https://example.com/pic.jpg',
    })
  })

  it('Should handle missing user properties in getUser', () => {
    const store = useAuthStore()
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    mockAuth0.user.value = {} as any

    const userAuth: UserAuth = store.getUser

    expect(userAuth.authId).toBeUndefined()
    expect(userAuth.firstName).toBeUndefined()
    expect(userAuth.lastName).toBeUndefined()
    expect(userAuth.accountPicture).toBeUndefined()
  })

  it('Should call loginWithRedirect on login', async () => {
    const store = useAuthStore()
    mockAuth0.loginWithRedirect.mockResolvedValue(undefined)

    await store.login()

    expect(mockAuth0.loginWithRedirect).toHaveBeenCalledWith({
      appState: {
        target: '/app/home',
      },
    })
  })

  it('Should handle login error', async () => {
    const store = useAuthStore()
    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {})
    mockAuth0.loginWithRedirect.mockRejectedValue(new Error('Login failed'))

    await store.login()

    expect(consoleSpy).toHaveBeenCalledWith('Login error:', expect.any(Error))
    consoleSpy.mockRestore()
  })

  it('Should call logout on logout', async () => {
    const store = useAuthStore()
    mockAuth0.logout.mockResolvedValue(undefined)

    await store.logout()

    expect(mockAuth0.logout).toHaveBeenCalledWith({
      logoutParams: { returnTo: window.location.origin },
    })
  })

  it('Should handle logout error', async () => {
    const store = useAuthStore()
    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {})
    mockAuth0.logout.mockRejectedValue(new Error('Logout failed'))

    await store.logout()

    expect(consoleSpy).toHaveBeenCalledWith('Logout error:', expect.any(Error))
    consoleSpy.mockRestore()
  })
})
