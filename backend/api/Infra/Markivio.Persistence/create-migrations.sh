#!/bin/bash

migration_name=$1

if [[ $? -ne 2 ]]; then
  echo "Give me a migratin name"
  exit -1
fi

dotnet-ef migrations add ${migration_name} --project ../Markivio.DbUpdater --startup-project ../Markivio.DbUpdater/
