import { useAuthStore } from '@/stores/AuthStore';
import { ApolloClient, ApolloLink, HttpLink, InMemoryCache } from '@apollo/client/core'

const authLink = new ApolloLink((operation, forward) => {
  const auhtStore = useAuthStore();
  const token = auhtStore.token;
  operation.setContext({
    headers: {
      authorization: token ? `Bearer ${token}` : "",
    }
  });

  return forward(operation);
});

const httpLink = new HttpLink({
  uri: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
})

const cache = new InMemoryCache();

export const apolloClient = new ApolloClient({
  link: authLink.concat(httpLink),
  cache: cache,

} as ApolloClient.Options)
