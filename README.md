# Cheermate

Cheermate is a desktop productivity assistant that manages TODO tasks and sends encouraging (cheer) messages to keep users motivated.

## Features (current / planned)
- Layered architecture (Domain / Application / Infrastructure / Presentation)
- WinForms UI (initial)
- Domain entities: TodoTask, Subtask, CheerMessage, User
- Repository + service abstractions (DateTimeProvider, GreetingService)
- EF Core persistence (planned / partial)
- Unit test project scaffold

## Architecture Overview
Layer responsibilities:
- Domain: Entities, enums, core rules (no external dependencies)
- Application: Interfaces, service orchestration, business use-cases
- Infrastructure: EF Core DbContext(s), repository implementations, external services
- Presentation: WinForms startup + UI
- Tests: Unit tests (focus on pure domain & application logic first)

## Project Layout
```
Cheermate.sln
Cheermate.Domain/
Cheermate.Application/
Cheermate.Infrastructure/
Cheermate.Presentation/
Cheermate.Tests/
```

## Getting Started
Prerequisites:
- .NET 8 SDK (see global.json if present)

Restore & build:
```
dotnet restore
dotnet build
```

Run WinForms:
```
dotnet run --project Cheermate.Presentation
```

## Configuration
App settings live under:
- Cheermate.Presentation/appsettings.json
- Cheermate.Infrastructure/appsettings.json (if needed)

Use user secrets locally for connection strings (avoid committing secrets):
```
cd Cheermate.Presentation
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Server=...;Database=Cheermate;..."
```

## EF Core (if using)
Create first migration:
```
dotnet ef migrations add InitialCreate -p Cheermate.Infrastructure -s Cheermate.Presentation
dotnet ef database update -p Cheermate.Infrastructure -s Cheermate.Presentation
```

## Testing
```
dotnet test
```
(Consider adding FluentAssertions and code coverage soon.)

## Contributing
1. Fork & clone
2. Create feature branch: `git checkout -b feat/short-description`
3. Commit with Conventional Commits (feat:, fix:, refactor:, chore:, test:)
4. Push & open a Pull Request

## Roadmap Ideas
- Replace duplicate DbContexts with a single CheermateDbContext
- Implement TaskPriority logic & domain validation
- Add Application layer use-case handlers
- Add CI pipeline (build + tests + coverage)
- Introduce code quality analyzers

## License
MIT (see LICENSE)
