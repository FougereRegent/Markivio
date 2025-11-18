// src/stores/authStore.js
import { defineStore } from 'pinia'
import { useAuth0 } from '@auth0/auth0-vue'
import { computed } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const auth0 = useAuth0()

  // -----------------------------
  // GETTERS (état "intégré" Auth0)
  // -----------------------------
  const isAuthenticated = computed(() => auth0.isAuthenticated.value)
  const user = computed(() => auth0.user.value)
  const isLoading = computed(() => auth0.isLoading.value)

  // -----------------------------
  // ACTIONS
  // -----------------------------
  async function login() {
    try {
      await auth0.loginWithRedirect()
    } catch (err) {
      console.error('Erreur login:', err)
    }
  }

  async function logout() {
    try {
      await auth0.logout({ logoutParams: { returnTo: window.location.origin }})
    } catch (err) {
      console.error('Erreur logout:', err)
    }
  }

  async function getToken() {
    try {
      return await auth0.getAccessTokenSilently()
    } catch (err) {
      console.error('Erreur token:', err)
      return null
    }
  }

  return {
    isAuthenticated,
    user,
    isLoading,
    login,
    logout,
    getToken
  }
})
