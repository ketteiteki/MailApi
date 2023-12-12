# Sample project using KeyCloak and Dapper

## How to start project

### Native
- Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Run `dotnet build`
- Install [Postgres](https://www.postgresql.org/download/)
- Create `keycloak` database
- Install [KeyCloak 22.0.5](https://www.keycloak.org/archive/downloads-22.0.5.html)
- Copy `keycloakConfig/realm.json` to `keycloak-22.0.5/data/import` directory
- Go to `keycloak-22.0.5/bin`
- Run `kc.[sh/bat] start-dev --import-realm --db postgres --db-username <username> --db-password <password>`

### Docker
- Run: `docker compose up`
