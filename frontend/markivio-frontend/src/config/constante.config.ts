export type Constant = {
  debounceTime: {
    buttonTime: number
    refetchTime: number
  }
  flickerTime: number
  toastTime: number
}

type Config = {
  audience: string
  domain: string
  clientId: string
  graphqlApi: string
}

export const CONST: Constant = {
  debounceTime: {
    buttonTime: 200,
    refetchTime: 200,
  },
  flickerTime: 300,
  toastTime: 3000,
}

export const CONFIG: Config = {
  audience: '',
  domain: '',
  clientId: '',
  graphqlApi: '',
}

if (import.meta.env.DEV) {
  CONFIG.audience = import.meta.env.VITE_MARKIVIO_AUTH_AUDIENCE
  CONFIG.domain = import.meta.env.VITE_MARKIVIO_AUTH_DOMAIN
  CONFIG.clientId = import.meta.env.VITE_MARKIVIO_AUTH_CLIENT_ID
  CONFIG.graphqlApi = import.meta.env.VITE_MARKIVIO_GRAPHQL_API
} else {
  const response = await fetch('/api/config')
  const payload = await response.json()

  CONFIG.clientId = payload['auth-client-id']
  CONFIG.domain = payload['auth-domain']
  CONFIG.audience = payload['auth-audience']
  CONFIG.graphqlApi = '/graphql'
  console.log(CONFIG);
}
