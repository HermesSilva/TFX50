# TFX50 AI Agent Instructions

This document guides AI agents in effectively working with the TFX50 codebase - a .NET 9 solution for web services and applications with TypeScript frontend.

## Architecture Overview

- **Core Component Structure**
  - `Application/Launcher`: Entry point that configures WebApplication, middleware, and module dependencies
  - `Core/TFX.Core`: Base utilities, controllers, and foundational services
  - `Core/TFX.Core.Data`: Data models and EF Core database access
  - `Core/TFX.Core.Access`: Authentication/session management using JWT
  - `Core/TFX.Core.UI`: Pure vanilla TypeScript frontend with zero external dependencies
  - `Modules/`: Optional extension modules (e.g., ESC, CRD, IMC)

## Key Development Patterns

1. **Module Registration**
   - All modules inherit from `XModule` and implement `Initialize(IServiceCollection)`
   - Modules are registered in `Application/Launcher/Program.cs`
   - Example: See `Core/TFX.Core/TFX.Core.Module.cs`

2. **Database Access**
   - Uses Entity Framework Core with micro-DBContext pattern:
     - Each module has its own dedicated DBContext for its tables
     - Each service with data manipulation has its own DBContext
     - Examples:
       - `TFXCoreDataContext`: Core domain models
       - `TFXESCCoreContext`: ESC module tables
       - `CEPxDBContext`: CEP service-specific data
     - This micro-DBContext approach ensures service isolation and modularity

3. **Authentication Flow**
   - JWT-based auth implemented in `TFX.Core.Access`
   - Session management via `XSessionManager.Initialize()`
   - CORS enabled for all origins in development

## Development Workflow

1. **Building the Project**
   ```powershell
   # Full solution build
   dotnet build TFX50.sln
   
   # UI build only (vanilla TypeScript)
   cd Core/TFX.Core.UI
   tsc # TypeScript compilation with native compiler
   ```

2. **Running the Application**
   ```powershell
   dotnet run --project Application/Launcher/Launcher.csproj
   ```
   Server runs on `http://localhost:7000`

3. **Testing**
   - Test projects named `.Test` (e.g., `TFX.Core.Test`)
   - Run tests with `dotnet test`
   - Note: Some tests require external service configuration

## Important Integration Points

1. **Frontend-Backend Communication**
   - Pure vanilla TypeScript frontend with no external dependencies
   - REST endpoints defined in controllers under `Core/TFX.Core/Controllers`
   - Static files served from `TFX.Core.UI/wwwroot`
   - All frontend functionality implemented using native TypeScript features only

2. **Module Extensions**
   - New modules should be added under `Modules/`
   - Must register module in `Program.cs` using dependency injection
   - Follow existing module patterns (e.g., `Projecao.Core.CRD`)

## Common Pitfalls

1. Content root is fixed to `/Tootega/Source/TFX50/Core/TFX.Core.UI`
2. Development requires DASE installation (see `DASE4VS.md`)
3. External service dependencies may need configuration for tests