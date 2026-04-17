import { defineStore } from 'pinia'
import { useAuth0 } from '@auth0/auth0-vue'
import { computed, ref } from 'vue'

export interface UserAuth {
  authId: string | undefined
  firstName: string | undefined
  lastName: string | undefined
  accountPicture: string | undefined
}

export const useAuthStore = defineStore('auth', () => {
  const auth = useAuth0()
  const token = ref('')
  const isAuthenticated = computed(() => auth.isAuthenticated.value)
  const user = computed(() => auth.user.value)
  const isLoading = computed(() => auth.isLoading.value)

  const getUser = computed((): UserAuth => {
    return {
      authId: user.value?.sub,
      firstName: user.value?.name,
      lastName: user.value?.family_name,
      accountPicture: user.value?.picture,
    }
  })

  async function init() {
    token.value = await auth.getAccessTokenSilently()
  }

  async function login() {
    try {
      await auth.loginWithRedirect({
        appState: {
          target: '/app/home',
        },
      })
    } catch (err) {
      console.error('Login error:', err)
    }
  }

  async function logout() {
    try {
      await auth.logout({ logoutParams: { returnTo: window.location.origin } })
    } catch (err) {
      console.error('Logout error:', err)
    }
  }

  return { token, isAuthenticated, user, isLoading, getUser, init, login, logout }
})
