# Configuration (Frontend)

## Variables d'environnement attendues

Le frontend lit ces variables Vite (via `import.meta.env`):

- `VITE_MARKIVIO_AUTH_DOMAIN` (Auth0 domain)
- `VITE_MARKIVIO_AUTH_CLIENT_ID` (Auth0 client id)
- `VITE_MARKIVIO_AUTH_AUDIENCE` (Auth0 audience)
- `VITE_MARKIVIO_GRAPHQL_API` (URL HTTP(S) de l'endpoint GraphQL, ex: `https://localhost:8080/graphql`)

## Exemple `.env.local`

Dans `frontend/markivio-frontend/.env.local`:

```bash
VITE_MARKIVIO_AUTH_DOMAIN="markivio.eu.auth0.com"
VITE_MARKIVIO_AUTH_CLIENT_ID="..."
VITE_MARKIVIO_AUTH_AUDIENCE="http://localhost:8080/"
VITE_MARKIVIO_GRAPHQL_API="https://localhost:8080/graphql"
```

