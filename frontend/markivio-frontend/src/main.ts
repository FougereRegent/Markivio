import { createApp } from 'vue'
import { createAuth0 } from '@auth0/auth0-vue';
import App from "./App.vue"
import router from './router'
import PrimeVue from 'primevue/config'
import { Button } from 'primevue';

const app = createApp(App)

const audience: string = import.meta.env.VITE_MARKIVIO_AUTH_AUDIENCE;
const domain: string = import.meta.env.VITE_MARKIVIO_AUTH_DOMAIN;
const clientId: string = import.meta.env.VITE_MARKIVIO_AUTH_CLIENT_ID;

console.log(audience);

app.use(router)
app.use(
  createAuth0({
    domain: domain,
    clientId: clientId,
    authorizationParams: {
      redirect_uri: window.location.origin,
      audience: audience,
    },
    useRefreshTokens: true,
  })
);
app.use(PrimeVue);
app.mount('#app')
