# TransactionService

Lightweight .NET 9 solution containing API, Worker, Console, and supporting projects for transaction processing.

## Prerequisites

- .NET SDK 9.0 or later
- A code editor (Visual Studio / VS Code / Rider)

## Projects

- `TransactionService.Api` — ASP.NET Core Web API
- `TransactionService.Worker` — Background worker/service
- `TransactionService.Console` — Console runner for manual tasks
- `TransactionService.Application` — Application services / use-cases
- `TransactionService.Domain` — Domain entities and logic
- `TransactionService.Infrastructure` — Persistence and external integrations
- `TransactionService.Tests` — Unit tests

## Build

Restore and build the solution:

```bash
dotnet restore
dotnet build --configuration Debug
```

## Run

- Run the API locally:

```bash
dotnet run --project TransactionService.Api
```

- Run the Worker locally:

```bash
dotnet run --project TransactionService.Worker
```

- Run the Console app:

```bash
dotnet run --project TransactionService.Console
```

Configuration is read from the `appsettings.json` files in each project. Development overrides are in `appsettings.Development.json` (ignored by `.gitignore`).

## Tests

Run unit tests:

```bash
dotnet test TransactionService.Tests
```

## Common workflows

- Create a branch, implement changes, run `dotnet build` and `dotnet test` before opening a PR.
- Keep secrets out of source control; use user secrets or environment variables for local development.

## Contributing

Feel free to open issues or PRs. Keep changes small and focused.

## License

Add a license file if needed.
