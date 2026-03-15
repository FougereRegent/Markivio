# Patterns et conventions

## Result pattern (FluentResults)

Dans `Application`, les use cases renvoient `Result` / `Result<T>`:

- succes: valeur retournee
- echec: `Error` typé (ex: `NotFoundError`, `AlreadyExistError`, `DomainError`) avec metadata `ERROR_CODE`

Dans `Presentation`, le helper `ThrowIfResultIsFailed()` transforme un echec en exception applicative, ensuite mappee proprement en erreur GraphQL.

## Domain validation via Value Objects

Le domaine modele les invariants via des Value Objects:

- `EmailValueObject`
- `IdentityValueObject`
- `TagValueObject`

Les VOs sont immutables "logiquement" (constructeurs validants + equality via `BaseValueObject`).

## Mapping: Mapperly

Mapperly est utilise pour:

- mapping entity -> DTO
- projection `IQueryable` -> DTO (evite de charger des entites inutiles)

Exemples:

- `ArticleMapperProjection.ProjectionToDto(...)`
- `TagMapperProjection.ProjectionToTagInformation(...)`

## Transactions par mutation GraphQL

Les mutations appliquent une transaction explicite via `UseTransactionMiddleware()`:

- ouvre une transaction
- execute le resolver
- commit + SaveChanges
- rollback en cas d'exception

## Tenancy / data isolation

Les query filters EF s'appuient sur `IAuthUser.CurrentUser` pour filtrer `Tag` et `Article`.

Consequences pratiques:

- ne pas executer de queries EF "hors contexte" (background jobs, console) sans initialiser `IAuthUser.CurrentUser`
- ne pas utiliser `AsNoTracking()` + update sans re-attacher (le repo `Update(...)` attache explicitement)

## Conventions de code deja presentes

- `nullable enable`, `implicit usings enable`
- separation nette des interfaces de repos (Domain) et des implementations (Infra)
- exceptions de domaine avec `ErrorCode` (utilise pour remonter un code d'erreur stable cote API)

