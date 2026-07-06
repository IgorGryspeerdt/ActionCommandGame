# ActionCommandGame

A C# school assignment project built as a multi-project solution using layered architecture.

> **Note on authorship:** This project was completed as part of a school assignment, and part of the codebase was provided as starter/template code. The implementation in this repository includes both starter code and assignment-specific additions.

## Overview

ActionCommandGame is organized into separate projects for API, services, repository/data access, models/DTOs, SDK, and console UI layers. This structure supports separation of concerns and makes it easier to test and extend the application.

From the solution layout, the project appears to include:
- Core domain/model and abstraction layers
- Service and service-model layers
- Repository/data access layer
- API layer
- Console UI applications
- Automated test projects

## Repository Structure

Top-level projects in this solution:

- `ActionCommandGame.Abstractions`
- `ActionCommandGame.Api`
- `ActionCommandGame.Configuration`
- `ActionCommandGame.Dto`
- `ActionCommandGame.Extensions`
- `ActionCommandGame.Helpers`
- `ActionCommandGame.Model`
- `ActionCommandGame.Repository`
- `ActionCommandGame.Sdk`
- `ActionCommandGame.Services.Abstractions`
- `ActionCommandGame.Services.Model`
- `ActionCommandGame.Services`
- `ActionCommandGame.Tests`
- `ActionCommandGame.Ui.Console`
- `ActionCommandGame.Ui.ConsoleApp`
- `actioncommandgame.Ui.Test`

Solution file:
- `ActionCommandGame.sln`

## Tech Stack

- **Language:** C#
- **Platform:** .NET (multi-project solution)

## Getting Started

### Prerequisites

- .NET SDK installed (recommended: latest LTS compatible with the solution)
- A C#-capable IDE such as Visual Studio / Rider / VS Code

### Clone the repository

```bash
git clone https://github.com/IgorGryspeerdt/ActionCommandGame.git
cd ActionCommandGame
```

### Build the solution

```bash
dotnet build ActionCommandGame.sln
```

### Run tests

```bash
dotnet test ActionCommandGame.sln
```

### Run applications

Depending on which entry point you want to execute:

```bash
dotnet run --project ActionCommandGame.Ui.Console
```

or

```bash
dotnet run --project ActionCommandGame.Ui.ConsoleApp
```

If the API project is configured as executable in your environment, you can run:

```bash
dotnet run --project ActionCommandGame.Api
```

## Assignment Context

This repository reflects coursework delivered as a school assignment.

- Some project scaffolding and baseline logic were provided by instructors.
- The completed solution includes custom implementation work, integration across layers, and test-related code.

If you are evaluating this project, please consider the above context when attributing architecture and implementation decisions.

## Possible Future Improvements

- Add detailed gameplay/rules documentation
- Add architecture diagram and request/data flow examples
- Expand test coverage reports and CI integration
- Add configuration/environment setup examples for API/UI runtime

## License

No explicit license file is currently included in the repository. If needed, add a `LICENSE` file to define usage terms.
