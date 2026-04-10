FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["GameDock_Backend/GameDock_Backend.csproj", "GameDock_Backend/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DTO/DTO.csproj", "DTO/"]

RUN dotnet restore "GameDock_Backend/GameDock_Backend.csproj"

COPY . .

WORKDIR "/src/GameDock_Backend"
RUN dotnet build "GameDock_Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameDock_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GameDock_Backend.dll"]