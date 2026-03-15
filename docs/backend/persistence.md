# Persistence (EF Core / PostgreSQL)

## DbContext

`Infra/Markivio.Persistence/Config/MarkivioContext.cs`:

- DbSets: `User`, `Article`, `Tag`, `Folder`
- Interceptor `DateTimeSaveUpdateIntercpetor`:
  - `CreatedAt` et `UpdatedAt` auto-set sur `Entity` lors de `SaveChanges`
- Configurations par entite via `IEntityTypeConfiguration`:
  - `ArticleDbConfiguration`, `TagDbConfiguration`, `UserDbConfiguration`, `FolderDbConfiguration`

## Migrations

Les migrations sont dans `Infra/Markivio.Persistence/Migrations`.

Important: l'API applique les migrations au demarrage (`db.Database.MigrateAsync()`).

## Model (high level)

```mermaid
erDiagram
  USERS ||--o{ ARTICLES : owns
  USERS ||--o{ TAGS : owns
  USERS ||--o{ FOLDERS : owns
  FOLDERS ||--o{ ARTICLES : contains

  USERS {
    uuid id
    string auth_id
    string email
    string username
    string first_name
    string last_name
    datetime created_at
    datetime updated_at
  }

  ARTICLES {
    uuid id
    string title
    jsonb article_content
    uuid user_id
    uuid folder_id
    datetime created_at
    datetime updated_at
  }

  TAGS {
    uuid id
    string name
    string color
    uuid user_id
    datetime created_at
    datetime updated_at
  }

  FOLDERS {
    uuid id
    string name
    uuid user_id
    datetime created_at
    datetime updated_at
  }
```

Notes:

- `ArticleContent` est configure via `OwnsOne(...).ToJson()` et `OwnsMany(Tags)` dans le JSON.
- `TagValueObject` est mappe via `ComplexProperty` (name/color).
- `User` mappe `EmailValueObject` et `IdentityValueObject` via `ComplexProperty`.

## Repositories + Unit of Work

- Interfaces dans `Domain/Markivio.Domain/Repositories/*`
- Implementations dans `Infra/Markivio.Persistence/Repositories/*`
- `IUnitOfWork` fournit une transaction explicite (utilisee par middleware GraphQL + interceptor user creation).

## Multi-tenancy / data isolation

`MarkivioContext` applique des `HasQueryFilter` sur `Tag` et `Article` base sur l'utilisateur courant.

Cela suppose:

- `IAuthUser.CurrentUser` est defini avant que la requete n'atteigne EF Core.
- Les resolvers/mutations doivent passer par le pipeline GraphQL normal (interceptor + middleware).

