# Execution locale (Frontend)

## Prerequis

- Node: voir `engines` dans `package.json` (`^20.19.0 || >=22.12.0`)
- `pnpm`

## Lancer en local (standalone)

Depuis `frontend/markivio-frontend`:

```bash
pnpm install
pnpm dev
```

Alternative (utile en container / LAN):

```bash
pnpm dev:custom
```

Notes:

- `dev:custom` force `--port 5173 --host 0.0.0.0`.

## Lancer via .NET Aspire (recommande pour dev full-stack)

Depuis `backend/api`:

```bash
dotnet run --project Presentation/Markivio.Aspire
```

Aspire demarre:

- Postgres + PgAdmin
- l'API GraphQL
- le frontend Vite (script `dev:custom`, port `5173`)

## Docker (dev)

Un `Dockerfile.dev` existe dans `frontend/markivio-frontend/` (Node + pnpm + `pnpm run dev`).

