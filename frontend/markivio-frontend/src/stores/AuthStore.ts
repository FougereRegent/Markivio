// src/stores/authStore.js
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
            isAuthenticated: auth.isAuthenticated,
            user: auth.user,
            isLoading: auth.isLoading
        };
    },
    actions: {
        login: async() => {
            try {
                await useAuth0().loginWithRedirect();
            } catch (err) {
                console.error('Erreur login:', err);
            }
        },

        logout: async () => {
            try {
                await useAuth0().logout({ logoutParams: { returnTo: window.location.origin }});
            } catch (err) {
                console.error('Erreur logout:', err);
            }
        },

        getToken: async() => {
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
            return  {
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