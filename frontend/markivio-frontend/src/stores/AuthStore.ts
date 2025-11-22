import { defineStore } from 'pinia'
import { useAuth0 } from '@auth0/auth0-vue'
import { computed, ref } from 'vue';

export interface UserAuth {
  authId: string | undefined,
  firstName: string | undefined,
  lastName: string | undefined,
  accountPicture: string | undefined,
}

export const useAuthStore = defineStore('auth', () => {
  const auth = useAuth0();
  const token = ref("")
  const isAuthenticated = computed(() => auth.isAuthenticated.value);
  const user = computed(() => auth.user.value)
  const isLoading = computed(() => auth.isLoading.value)

  const getUser = computed((): UserAuth => {
    return {
      authId: user.value?.sub,
      firstName: user.value?.name,
      lastName: user.value?.family_name,
      accountPicture: user.value?.picture,
      //      };
    }
  });

  async function init() {
    token.value = await auth.getAccessTokenSilently();
  }

  async function login() {
    try {
      await auth.loginWithRedirect({
        appState: {
          target: "/app/home"
        }
      });
    } catch (err) {
      console.error('Erreur login:', err);
    }
  }

  async function logout() {
    try {
      await auth.logout({ logoutParams: { returnTo: window.location.origin } });
    } catch (err) {
      console.error('Erreur logout:', err);
    }
  }

  return { token, isAuthenticated, user, isLoading, getUser, init, login, logout };
})

//export const useAuthStore = defineStore('auth', {
//  state: () => {
//    const auth = useAuth0();
//
//    return {
//      auth: auth,
//      isAuthenticated: computed(() => auth.isAuthenticated),
//      user: computed(() => auth.user),
//      isLoading: computed(() => auth.isLoading),
//      token: null as string | null,
//    };
//  },
//  actions: {
//
//    async init() {
//      const token = await this.auth.getAccessTokenSilently();
//      this.token = token;
//    },
//
//    async login() {
//      try {
//        await this.auth.loginWithRedirect({
//          appState: {
//            target: "/app/home"
//          }
//        });
//      } catch (err) {
//        console.error('Erreur login:', err);
//      }
//    },
//
//    async logout() {
//      try {
//        await this.auth.logout({ logoutParams: { returnTo: window.location.origin } });
//      } catch (err) {
//        console.error('Erreur logout:', err);
//      }
//    },
//  },
//  getters: {
//    isLoggedIn: state => {
//      return state.auth.isAuthenticated;
//    },
//    getUser(state): UserAuth {
//      return {
//        authId: state.user.value?.sub,
//        firstName: state.user.value?.name,
//        lastName: state.user.value?.family_name,
//        accountPicture: state.user.value?.picture,
//      };
//    },
//    loading: state => {
//      return state.isLoading;
//    },
//  }
//})
