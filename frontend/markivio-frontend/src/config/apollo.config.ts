import { useAuthStore } from '@/stores/auth-store';
import { ApolloClient, ApolloLink, HttpLink, InMemoryCache } from '@apollo/client/core';

const authLink = new ApolloLink((operation, forward) => {
  const authStore = useAuthStore();
  const token = authStore.token;
  operation.setContext({
    headers: {
      authorization: token ? `Bearer ${token}` : '',
    },
  });

  return forward(operation);
});

const httpLink = new HttpLink({
  uri: import.meta.env.VITE_MARKIVIO_GRAPHQL_API,
});

const cache = new InMemoryCache({
});

export const apolloClient = new ApolloClient({
  link: authLink.concat(httpLink),
  cache: cache,
} as ApolloClient.Options);
