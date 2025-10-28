# TFX50 AI Agent Instructions

This document guides AI agents in effectively working with the TFX50 codebase - a .NET 9 solution for web services and applications with TypeScript frontend.

## Architecture Overview

- **Core Component Structure**
  - `TFX.Core.UI`: Pure vanilla TypeScript frontend with zero external dependencies

## Development Workflow

1. **Building the Project**
   ```powershell
   # Full solution build
   dotnet build TFX.Core.UI.sln
   
   # UI build only (vanilla TypeScript)
   cd ./
   tsc # TypeScript compilation with native compiler
   ```
   - Note: Some tests require external service configuration

## Important Integration Points

1. **Frontend-Backend Communication**
   - Pure vanilla TypeScript frontend with no external dependencies
   - Static files served from `wwwroot`
   - All frontend functionality implemented using native TypeScript features only

## Common Pitfalls

1. Content root is fixed to `TFX.Core.UI`
