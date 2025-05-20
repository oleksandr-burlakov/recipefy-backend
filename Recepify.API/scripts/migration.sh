#!/bin/bash

MIGRATION_NAME=$1

dotnet ef migrations add "${MIGRATION_NAME}" --project Recepify.DLL/Recepify.DLL.csproj --startup-project Recepify.API/Recepify.API.csproj