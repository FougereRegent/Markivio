import { cacheExchange, Client, fetchExchange } from '@urql/vue'
import { authExchange, type AuthConfig } from '@urql/exchange-auth'
import { useAuthStore } from '@/stores/auth-store'
import { CONFIG } from './constante.config'

async function initializeAuthState() {
  const auth = useAuthStore()
  await auth.init()
  const token = auth.token
  return token
}

export const httpClient = new Client({
  url: CONFIG.graphqlApi,
  exchanges: [
    authExchange(async (utils) => {
      const token = await initializeAuthState()
      return {
        addAuthToOperation(operation) {
          if (!token) return operation
          return utils.appendHeaders(operation, {
            Authorization: `Bearer ${token}`,
          })
        },
      } as AuthConfig
    }),
    fetchExchange,
  ],
})
