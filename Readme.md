# ReadingIsGood - Modern .NET 5 Web API Example

ReadingIsGood is a modern Web API example with [Domain-Driven Design](https://docs.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice) modeling.

## Features

- [Entity Framework Core](https://docs.microsoft.com/ef/core/) (Code First)
- [PostgreSQL](https://www.postgresql.org/) (EF Core -> [Npgsql](https://www.npgsql.org/))
- Native .NET Identity Database Schema Integration
- JWT authentication with Microsoft AspNetCore Identity package
- Swagger + OpenAPI (V3.0.3) + JWT authentication support
- [XUnit](https://xunit.net/) Test Layer + Sample Integration Test Scenerios
- Docker support for containerization
  
## How to use it?

### Docker Way

```console
docker-compose up -d
```

### Classic

```console
 cd ReadingIsGood.API
 dotnet restore "ReadingIsGood.API.csproj"
 dotnet run
```

## Notes

- Before deployment **remove Swagger definitions** in Configure section. It enabled for demonstration purposes only
