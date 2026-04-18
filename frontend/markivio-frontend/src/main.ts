import { createApp } from 'vue'
import { createAuth0 } from '@auth0/auth0-vue'
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config'
import MyPreset from './themes/themes'
import './assets/style.css'
import { createPinia } from 'pinia'
import * as z from 'zod'
import { ToastService } from 'primevue'
import { httpClient } from './config/urql.config'
import { CONFIG } from './config/constante.config'
import urql from '@urql/vue'

const app = createApp(App)
const pinia = createPinia()

z.config(z.locales.fr())

app
  .use(router)
  .use(ToastService)
  .use(pinia)
  .use(
    createAuth0({
      domain: CONFIG.domain,
      clientId: CONFIG.clientId,
      authorizationParams: {
        redirect_uri: `${window.location.origin}/callback`,
        audience: CONFIG.audience,
      },
      useRefreshTokens: true,
    }),
  )
  .use(PrimeVue, {
    theme: {
      preset: MyPreset,
      options: {
        prefix: 'p',
        darkModeSelector: '.app-dark',
      },
    },
  })
  .use(urql, httpClient)
  .mount('#app')
