import {cacheExchange, Client, fetchExchange} from '@urql/vue';
import { authExchange, type AuthConfig } from '@urql/exchange-auth';
import { useAuthStore } from '@/stores/auth-store';

async function initializeAuthState() {
  const auth = useAuthStore();
  debugger;
  await auth.init();
  const token = auth.token;
  return token;
}

export const httpClient = new Client({
  url: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
  exchanges: [cacheExchange, authExchange(async utils => {
    let token = await initializeAuthState();
    return {
      addAuthToOperation(operation) {
          if(!token) return operation;
          return utils.appendHeaders(operation, {
            Authorization: `Bearer ${token}`
          });
      },
    } as AuthConfig;
  }), fetchExchange],
})
