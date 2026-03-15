# Configuration

## Variables d'environnement

L'API lit explicitement:

- `MARKIVIO_AUTHORITY`: issuer/authority pour la validation JWT (`AddJwtBearer`).
- `MARKIVIO_AUDIENCE`: audience attendue (mais la validation audience est desactivee).
- `ConnectionStrings__markivio`: connection string PostgreSQL utilisee par EF Core.

Le `Program.cs` construit un `EnvConfig` a partir de ces valeurs.

## Connection string: cle attendue

Le code lit:

```csharp
builder.Configuration.GetConnectionString("markivio")
```

Donc en env vars il faut:

- `ConnectionStrings__markivio="Host=...;Port=5432;Database=markivio;Username=...;Password=..."`

Note: `Presentation/Markivio.GraphQl/appsettings.json` contient `ConnectionStrings:DefaultConnection` (non utilise par la cle `markivio`).

## Ports et URLs (dev)

Selon `Presentation/Markivio.GraphQl/Properties/launchSettings.json`:

- HTTPS: `https://localhost:8080`
- HTTP: `http://localhost:8081`

GraphQL:

- `POST /graphql`

## Auth (JWT Bearer)

Configuration actuelle:

- `Authority = MARKIVIO_AUTHORITY`
- `Audience = MARKIVIO_AUDIENCE`
- `TokenValidationParameters.ValidateAudience = false`

Consquence: la presence d'un token valide (issuer + signature) est critique; l'audience n'est pas bloquante.

