# eshop Clean Architecture

Welcome to the eShop Clean Architecture project! This repository demonstrates the implementation of a robust e-commerce web application using the principles of Clean Architecture in .NET. It combines modern software design patterns, technologies, and best practices to deliver a maintainable, scalable, highly modular and production ready application.

# Key Features

- **Rich Domain Model**: Utilizes entities and value objects to create a domain model that reflects the business logic.
- **Static Factory Pattern**: Demonstrates the use of static factory methods to create objects.
- **Domain Events**: Implements domain events to enable loose coupling between domain entities.
- **Double Dispatch**: Uses double dispatch for handling domain events efficiently.
- **Result and Error Handling**: Utilizes Result and Error classes for handling success and error scenarios.
- **MediatR**: Implements the Mediator pattern with MediatR for decoupled communication between application components.
- **CQRS (Command Query Responsibility Segregation)**: Separates read and write operations using CQRS for improved performance and maintainability.
- **Domain Event Handlers**: Demonstrates domain event handling to trigger side effects. 
- **Queries with Dapper**: Performs data queries efficiently using Dapper.
- **Commands with Entity Framework Core**: Executes data modification operations using Entity Framework Core.
- **Logging**: Solves logging as a cross-cutting concern for better visibility into application behavior.
- **Validation Pipeline**: Implements a validation pipeline for commands to ensure data integrity.
- **Domain Entity Configuration**: Provides configuration for domain entities.
- **Global Exception Handling Middleware**: Handles exceptions gracefully with global middleware.

# Prerequisites

Before getting started, make sure you have the following prerequisites installed:

- [.Net7](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Docker](https://www.docker.com/)

# Getting Started

Follow these steps to set up and run the eShop Clean Architecture project:

```
git clone https://github.com/saimankhatiwada/eshop.git
```

```
cd eshop
```

```
docker compose up
```

Access the web application at `http://localhost:4000`

# Contributing
We welcome contributions from the community! If you find issues, have suggestions, or want to add new features, please open a GitHub issue or submit a pull request.

# License
This project is licensed under the Apache License - see the [LICENSE](./LICENSE) file for details.

# Acknowledgments
- This project draws inspiration from the Clean Architecture principles and various design patterns.
- Special thanks to the .NET and open-source community for their valuable contributions and libraries.