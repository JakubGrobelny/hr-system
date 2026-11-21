# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first, so the dependency layer is cached between source changes.
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/HrSystem.Domain/HrSystem.Domain.csproj src/HrSystem.Domain/
COPY src/HrSystem.Application/HrSystem.Application.csproj src/HrSystem.Application/
COPY src/HrSystem.Infrastructure/HrSystem.Infrastructure.csproj src/HrSystem.Infrastructure/
COPY src/HrSystem.Api/HrSystem.Api.csproj src/HrSystem.Api/
RUN dotnet restore src/HrSystem.Api/HrSystem.Api.csproj

COPY src/ src/
RUN dotnet publish src/HrSystem.Api/HrSystem.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "HrSystem.Api.dll"]
