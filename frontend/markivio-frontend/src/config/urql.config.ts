import {cacheExchange, Client, fetchExchange} from '@urql/vue';

function getToken(): string | null {
  return "";
}

export const httpClient = new Client({
  url: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
  exchanges: [cacheExchange, fetchExchange],
  fetchOptions: () => {
    const token = getToken();
    return {
      headers: {
        authorization: token ? `Bearer ${token}` : ''
      },
    }
  }
})
