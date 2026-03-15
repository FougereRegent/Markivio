# Execution locale

## Pre-requis

- .NET SDK 10 (`dotnet --info`)
- (optionnel) Docker si vous utilisez `Dockerfile.Api`
- Pour le mode Aspire: `pnpm` et le repo frontend a l'emplacement attendu (`../../../../frontend/markivio-frontend`)

## Dev local recommande: .NET Aspire

L'AppHost demarre:

- PostgreSQL + PgAdmin (port 8085)
- l'API GraphQL (`https://localhost:8080/graphql`)
- le frontend Vite (port 5173)

Commande:

```bash
dotnet run --project Presentation/Markivio.Aspire
```

Les variables d'environnement sont injectees depuis `Presentation/Markivio.Aspire/appsettings.Development.json`.

## Lancer uniquement l'API GraphQL

```bash
dotnet run --project Presentation/Markivio.GraphQl
```

Variables minimales attendues (voir [Configuration](/home/damien/Documents/info/Markivio/backend/api/docs/configuration.md)):

- `MARKIVIO_AUTHORITY`
- `MARKIVIO_AUDIENCE`
- `ConnectionStrings__markivio` (ou equivalent, cf. note)

En dev, l'API expose aussi OpenAPI + Scalar:

- `GET /openapi/v1.json`
- `GET /docs` (Scalar)

## Docker (API)

Build:

```bash
docker build -f Dockerfile.Api -t markivio-api .
```

Run (exemple, variables a adapter):

```bash
docker run --rm -p 8080:8080 \
  -e ASPNETCORE_URLS=http://+:8080 \
  -e MARKIVIO_AUTHORITY="https://markivio.eu.auth0.com/" \
  -e MARKIVIO_AUDIENCE="https://localhost:8080/" \
  -e ConnectionStrings__markivio="Host=...;Port=5432;Database=markivio;Username=...;Password=..." \
  markivio-api
```

