FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["GameDock.API/GameDock.API.csproj", "GameDock.API/"]
COPY ["GameDock.BLL/GameDock.BLL.csproj", "GameDock.BLL/"]
COPY ["GameDock.DAL/GameDock.DAL.csproj", "GameDock.DAL/"]
COPY ["GameDock.Domain/GameDock.Domain.csproj", "GameDock.Domain/"]
COPY ["GameDock.DTO/GameDock.DTO.csproj", "GameDock.DTO/"]
COPY ["GameDock.Shared/GameDock.Shared.csproj", "GameDock.Shared/"]

RUN dotnet restore "GameDock.API/GameDock.API.csproj"

COPY . .

WORKDIR "/src/GameDock.API"
RUN dotnet publish "GameDock.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GameDock.API.dll"]