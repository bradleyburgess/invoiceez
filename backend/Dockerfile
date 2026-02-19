# ===== Base image =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 5000

# ===== Build image =====
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy solution and project files (for better caching)
COPY Invoiceez.slnx ./
COPY Api/Api.csproj Api/
COPY Logic/Logic.csproj Logic/

# Restore dependencies
RUN dotnet restore Invoiceez.slnx

# Copy the rest of the source
COPY . .

# Publish the app
RUN dotnet publish Invoiceez.slnx -c Release -o /app/publish

# ===== Final image =====
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh && \
    addgroup -g 1000 invoiceez && \
    adduser -u 1000 -G invoiceez -D invoiceez && \
    apk add --no-cache icu-data-full icu-libs
USER invoiceez

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["./entrypoint.sh"]
