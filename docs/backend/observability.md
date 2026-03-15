# Observabilite

## OpenTelemetry

Configure dans `Presentation/Markivio.GraphQl/Config/GraphQlConfig.cs`:

- Traces:
  - ASP.NET Core
  - HttpClient
  - HotChocolate
  - EntityFrameworkCore
  - Exporter OTLP (`AddOtlpExporter`)
- Metrics:
  - HttpClient
  - ASP.NET Core
  - Exporter OTLP

## Logs (Serilog)

Configure dans `Presentation/Markivio.GraphQl/Config/ServicesConfig.cs`:

- Serilog
  - Console sink (async)
  - OpenTelemetry sink (async)

Le `DomainErrorFilter` log:

- warnings pour les erreurs "fonctionnelles" (ApplicationLayerException)
- errors pour les exceptions inattendues
- `traceId` via `Activity.Current`

## Aspire

Le projet `Presentation/Markivio.Aspire` est un AppHost qui:

- demarre Postgres + PgAdmin
- demarre l'API
- demarre le frontend

Il facilite aussi l'exploration OTel via le dashboard Aspire (voir launchSettings Aspire).

