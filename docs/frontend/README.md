# Markivio Frontend - Documentation

## TL;DR

- Stack: `Vue 3` + `Vite` + `TypeScript` + `pnpm`
- UI: `PrimeVue` + `TailwindCSS`
- State: `Pinia`
- Auth: `Auth0` via `@auth0/auth0-vue`
- API: `Apollo Client` vers l'API GraphQL (`/graphql`)

## Sommaire

- [Execution locale](./running.md)
- [Configuration (env vars)](./configuration.md)
- [Architecture (points d'entree, auth, API)](./architecture.md)

## Repo layout (frontend)

- `frontend/markivio-frontend/`
- `src/main.ts`: bootstrap Vue, Auth0, Pinia, PrimeVue
- `src/config/apollo.config.ts`: client Apollo (header `Authorization: Bearer <token>`)
- `src/graphql/*.queries.ts`: documents GraphQL utilises par les services
- `src/services/*.service.ts`: appels API + mapping erreurs

