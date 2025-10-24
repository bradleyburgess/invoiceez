#!/bin/sh
set -e

echo "⏳ Waiting for database to be ready..."

# simple wait loop for Postgres
while ! nc -z invoiceez_postgres 5432; do
  sleep 1
done

# echo "✅ Database is up — running migrations..."
# dotnet ef database update

echo "🚀 Starting application..."
exec dotnet Api.dll
