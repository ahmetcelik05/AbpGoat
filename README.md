# AbpGoat

AbpGoat is a sample [ABP Framework](https://abp.io) application built on ASP.NET Core. It
provides a small back office — books and authors, documents, reporting, feedback, notes and
orders — on top of the standard ABP module set (identity, tenant management, permission
management, feature management, OpenIddict and audit logging).

It uses Razor Pages / MVC with the LeptonX Lite theme and PostgreSQL for storage.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) running locally
- [ABP CLI](https://abp.io/docs/latest/cli): `dotnet tool install -g Volo.Abp.Studio.Cli`
- [Node.js](https://nodejs.org/) (LTS) for client-side libraries

## Getting started

1. **Configure the database.** Update the `ConnectionStrings:Default` value in
   `src/AbpGoat.Web/appsettings.json` and `src/AbpGoat.DbMigrator/appsettings.json` if your
   PostgreSQL settings differ from the defaults.

2. **Restore client-side libraries** (run once, from the `src/AbpGoat.Web` folder):

   ```bash
   abp install-libs
   ```

3. **Create and seed the database** by running the DbMigrator:

   ```bash
   dotnet run --project src/AbpGoat.DbMigrator
   ```

4. **Run the web application:**

   ```bash
   dotnet run --project src/AbpGoat.Web
   ```

   The app is served at `https://localhost:44394` by default.

The default administrator account is `admin` / `1q2w3E*`.

## Solution structure

The solution follows ABP's layered architecture:

- `AbpGoat.Domain` / `AbpGoat.Domain.Shared` — entities, domain services and shared constants.
- `AbpGoat.Application` / `AbpGoat.Application.Contracts` — application services, DTOs and
  their contracts.
- `AbpGoat.EntityFrameworkCore` — the `DbContext`, entity mappings and EF Core migrations.
- `AbpGoat.HttpApi` / `AbpGoat.HttpApi.Client` — auto-generated HTTP API controllers and
  client proxies.
- `AbpGoat.Web` — the Razor Pages / MVC UI host.
- `AbpGoat.DbMigrator` — a console app that applies migrations and seeds initial data.

## Useful links

- ABP Framework documentation: https://abp.io/docs
- Getting started with ABP: https://abp.io/docs/latest/tutorials
