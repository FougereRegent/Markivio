# Markivio Backend API (GraphQL) - Documentation

Cette documentation décrit l'architecture, les choix techniques, et les conventions du repo `backend/api`.

## TL;DR

- Stack: `.NET 10` + `ASP.NET Core` + `HotChocolate GraphQL` + `EF Core (PostgreSQL/Npgsql)` + `Auth0 (JWT Bearer)`
- Observabilite: `OpenTelemetry` + `Serilog` (export OTLP, Aspire friendly)
- Orchestration dev: `.NET Aspire AppHost` (Postgres + PgAdmin + API + Vite frontend)

## Sommaire

- [Architecture](/home/damien/Documents/info/Markivio/backend/api/docs/architecture.md)
- [Libs / dependances (NuGet)](/home/damien/Documents/info/Markivio/backend/api/docs/libs.md)
- [Execution locale (Aspire, API seule, Docker)](/home/damien/Documents/info/Markivio/backend/api/docs/running.md)
- [Configuration (env vars, connection string)](/home/damien/Documents/info/Markivio/backend/api/docs/configuration.md)
- [GraphQL (schema, auth, paging, erreurs)](/home/damien/Documents/info/Markivio/backend/api/docs/graphql.md)
- [Persistence (EF Core, migrations, multi-tenancy)](/home/damien/Documents/info/Markivio/backend/api/docs/persistence.md)
- [Observabilite (OTel/Serilog/Aspire)](/home/damien/Documents/info/Markivio/backend/api/docs/observability.md)
- [Tests](/home/damien/Documents/info/Markivio/backend/api/docs/testing.md)
- [Patterns et conventions](/home/damien/Documents/info/Markivio/backend/api/docs/patterns.md)

## Diagramme de contexte (runtime)

```mermaid
flowchart LR
  U[Utilisateur] --> FE[Frontend (Vite)]
  FE -->|HTTPS /graphql| API[Markivio.GraphQl\nASP.NET Core + HotChocolate]
  API -->|Npgsql / EF Core| PG[(PostgreSQL)]
  API -->|JWT Bearer validation| AUTH0[(Auth0)]
  API -->|OTLP| OTEL[(Collector / Aspire Dashboard)]

  subgraph DEV[Dev via .NET Aspire]
    AH[Markivio.Aspire AppHost] --> PG
    AH --> API
    AH --> FE
  end
```
