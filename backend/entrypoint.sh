#!/bin/sh
set -e

echo "⏳ Waiting for database to be ready..."

# simple wait loop for Postgres
while ! nc -z -w 1 invoiceez_postgres 5432; do
  sleep 1
done

echo "✅ Database is up."
echo "🚀 Starting application..."

dotnet Api.dll 2>&1
EXIT_CODE=$?
echo "💥 Application exited with code $EXIT_CODE"
exit $EXIT_CODE
