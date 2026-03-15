# Tests

## Commandes

```bash
dotnet test
```

Le script `test-as-fail.sh` permet de boucler les tests jusqu'a un echec (utile pour traquer de la flakiness):

```bash
./test-as-fail.sh
```

## Stack de test

- xUnit v3
- Moq
- Shouldly
- Bogus (data generation)

## Organisation

`Tests/Markivio.UnitTests` contient des tests par couche:

- `Domain/*`: validation des Value Objects / Entities.
- `Application/*`: use cases (mock repos + auth user).
- `Infra/*`: tests du UnitOfWork.

