# GraphQL

## Endpoint

- `POST /graphql`
- En dev: l'UI/Schema se fait via les outils HotChocolate (selon config) et les docs HTTP via Scalar (`/docs`).

## Schema (structure)

Le schema est compose via HotChocolate:

- Query: `Presentation/Markivio.GraphQl/GraphQl/Query.cs`
- Mutation: `Presentation/Markivio.GraphQl/GraphQl/Mutation.cs`
- Types: `Presentation/Markivio.GraphQl/GraphQl/*` (ex: `UserInformationType`, `ArticleInformationType`, ...)

Paging:

- Queries `articles` et `tags` utilisent `UseOffsetPaging(...)` avec des options differentes (total count, boundaries).

## Authentification / autorisation

- `QueryType` et `MutationType` sont `.Authorize()` (tout est protege).
- JWT Bearer est configure via `AddAuth0(...)`.

### Injection du "CurrentUser"

Le couple:

- `AuthUserInterceptor` (HTTP request interceptor)
- `AuthMiddleware` (field middleware)

permet de:

1. Lire le `Bearer token` des headers.
2. Verifier si l'utilisateur existe en base (`IUserRepository.GetUserByAuthId`).
3. Creer l'utilisateur si absent (transaction explicite).
4. Mettre l'utilisateur dans le `GlobalState` HotChocolate (`auth-user`).
5. Pousser l'utilisateur dans `IAuthUser.CurrentUser` avant l'execution des resolvers.

## Transactions

Les mutations sont entourees d'un middleware transactionnel (par champ GraphQL):

- `UseTransactionMiddleware()` -> `BeginTransaction`, execute resolver, `SaveChanges` ou `Rollback`.

Cela permet d'avoir une transaction par mutation (utile pour EF Core).

## Gestion d'erreurs

Pattern dominant:

- Application renvoie `FluentResults.Result` / `Result<T>`.
- Presentation appelle `ThrowIfResultIsFailed()` et leve `ApplicationLayerException`.
- `DomainErrorFilter` intercepte et transforme l'erreur GraphQL:
  - code = `ErrorCode` present dans `Metadata` des `Error` FluentResults.
  - logs avec `traceId` (Activity).

## Exemples

### Query "me"

```graphql
query {
  me {
    id
    firstName
    lastName
    email
  }
}
```

### Query "articles" (offset paging)

```graphql
query {
  articles(title: "Doc", tags: ["csharp"]) {
    totalCount
    items {
      id
      title
      source
      tags { name color }
    }
  }
}
```

