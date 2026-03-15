# Architecture (Frontend)

## Bootstrap

Le point d'entree est `src/main.ts`:

- configure Auth0 (`@auth0/auth0-vue`) avec `redirect_uri` sur `/callback`
- installe `Pinia`
- installe PrimeVue + theme custom

## Appels API (GraphQL)

- Le client Apollo est configure dans `src/config/apollo.config.ts`.
- Le token est injecte dans l'en-tete `Authorization` via un `ApolloLink`.

Les documents GraphQL sont dans `src/graphql/*.queries.ts` (exemples):

- `query Me { me { ... } }`
- `query Articles($skip, $take) { articles { items totalCount pageInfo } }`
- `mutation UpdateMyUser(...)`
- `mutation AddArticles(...)`
- `mutation CreateTag(...)`

## Auth

- Les routes sont protegees via `authGuard` (Auth0) dans `src/router/index.ts`.
- Le token est stocke/lu via un store (`src/stores/auth-store.ts`).

