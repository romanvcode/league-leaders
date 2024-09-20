FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/LeagueLeaders.API/LeagueLeaders.API.csproj", "LeagueLeaders.API/"]
COPY ["src/LeagueLeaders.Application/LeagueLeaders.Application.csproj", "LeagueLeaders.Application/"]
COPY ["src/LeagueLeaders.Domain/LeagueLeaders.Domain.csproj", "LeagueLeaders.Domain/"]
COPY ["src/LeagueLeaders.Infrastructure/LeagueLeaders.Infrastructure.csproj", "LeagueLeaders.Infrastructure/"]
RUN dotnet restore "./LeagueLeaders.API/LeagueLeaders.API.csproj"
COPY ./src ./
WORKDIR "/src/LeagueLeaders.API"
RUN dotnet build "./LeagueLeaders.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LeagueLeaders.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeagueLeaders.API.dll"]