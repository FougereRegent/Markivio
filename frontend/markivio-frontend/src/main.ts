import { createApp } from 'vue'
import { createAuth0 } from '@auth0/auth0-vue';
import App from "./App.vue"
import router from './router'
import PrimeVue from 'primevue/config'
import MyPreset from './themes/themes';
import './assets/style.css';
import { createPinia } from 'pinia';

const app = createApp(App);
const pinia = createPinia();

const audience: string = import.meta.env.VITE_MARKIVIO_AUTH_AUDIENCE;
const domain: string = import.meta.env.VITE_MARKIVIO_AUTH_DOMAIN;
const clientId: string = import.meta.env.VITE_MARKIVIO_AUTH_CLIENT_ID;


app.use(router)
  .use(pinia)
  .use(
    createAuth0({
      domain: domain,
      clientId: clientId,
      authorizationParams: {
        redirect_uri: `${window.location.origin}/callback`,
        audience: audience,
      },
      useRefreshTokens: true,
    })
  ).use(PrimeVue, {
    theme: {
      preset: MyPreset,
      options: {
        prefix: 'p',
        darkModeSelector: '.app-dark'
      }
    }
  }).mount('#app')
