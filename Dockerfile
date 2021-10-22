ARG BuildConfiguration=Debug

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine AS base
ARG BuildConfiguration

FROM base as build
ARG BuildConfiguration
WORKDIR /build
COPY Api/Api.csproj ./Api/
COPY tests/**/Api.Students.IntegrationTests.csproj ./tests/Api.Students.IntegrationTests/
COPY tests/**/Api.Backoffice.IntegrationTests.csproj ./tests/Api.Backoffice.IntegrationTests/
COPY tests/**/Api.Configuration.IntegrationTests.csproj ./tests/Api.Configuration.IntegrationTests/
COPY tests/**/Api.Tests.csproj ./tests/Api.Tests/
COPY Tekman.Primaria.sln .

RUN dotnet restore Tekman.Primaria.sln

COPY . .

WORKDIR /build
RUN dotnet build Tekman.Primaria.sln --configuration $BuildConfiguration --no-restore

FROM build AS test
WORKDIR /build

FROM build AS publish
ARG BuildConfiguration
WORKDIR /build/Api/
RUN dotnet publish --configuration $BuildConfiguration -o /app --no-build
WORKDIR /build

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-alpine AS final
EXPOSE 80
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "Api.dll"]