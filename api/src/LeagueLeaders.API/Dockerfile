FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/LeagueLeaders.API/LeagueLeaders.API.csproj", "src/LeagueLeaders.API/"]
COPY ["src/LeagueLeaders.Application/LeagueLeaders.Application.csproj", "src/LeagueLeaders.Application/"]
COPY ["src/LeagueLeaders.Domain/LeagueLeaders.Domain.csproj", "src/LeagueLeaders.Domain/"]
COPY ["src/LeagueLeaders.Infrastructure/LeagueLeaders.Infrastructure.csproj", "src/LeagueLeaders.Infrastructure/"]
RUN dotnet restore "./src/LeagueLeaders.API/LeagueLeaders.API.csproj"
COPY . .
WORKDIR "/src/src/LeagueLeaders.API"
RUN dotnet build "./LeagueLeaders.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LeagueLeaders.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeagueLeaders.API.dll"]