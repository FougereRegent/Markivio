import { defineStore } from 'pinia'
import { useAuth0 } from '@auth0/auth0-vue'

export interface UserAuth {
  authId: string,
  firstName: string,
  lastName: string,
  accountPicture: string,
}

export const useAuthStore = defineStore('auth', {
  state: () => {
    const auth = useAuth0();

    return {
      auth: auth,
      isAuthenticated: auth.isAuthenticated,
      user: auth.user,
      isLoading: auth.isLoading
    };
  },
  actions: {
    async login() {
      try {
        await this.auth.loginWithRedirect();
      } catch (err) {
        console.error('Erreur login:', err);
      }
    },

    async logout() {
      try {
        await useAuth0().logout({ logoutParams: { returnTo: window.location.origin } });
      } catch (err) {
        console.error('Erreur logout:', err);
      }
    },

    async getToken() {
      try {
        return await useAuth0().getAccessTokenSilently();
      } catch (err) {
        console.error('Erreur token:', err);
        return null
      }
    },
  },
  getters: {
    isLoggedIn: state => {
      return state.isAuthenticated;
    },
    getUser(state): UserAuth {
      return {
        authId: state.user?.sub ?? "",
        firstName: state.user?.name ?? "",
        lastName: state.user?.family_name ?? "",
        accountPicture: state.user?.picture ?? "",
      };
    },
    loading: state => {
      return state.isLoading;
    }
  }
});
