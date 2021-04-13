FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
COPY . /app

WORKDIR /app/ReadingIsGood.Domain
RUN dotnet restore "ReadingIsGood.Domain.csproj"

WORKDIR /app/ReadingIsGood.Infrastructure
RUN dotnet restore "ReadingIsGood.Infrastructure.csproj"

WORKDIR /app/ReadingIsGood.API
RUN dotnet restore "ReadingIsGood.API.csproj"
RUN dotnet build "ReadingIsGood.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReadingIsGood.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReadingIsGood.API.dll"]