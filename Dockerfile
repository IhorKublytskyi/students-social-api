FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY backend.API/backend.API.csproj ./backend.API/
COPY backend.Persistence/backend.Persistence.csprojproj ./backend.Persistence/
COPY backend.Core/backend.Core.csproj ./backend.Core/
COPY backend.Infrastructure/backend.Infrastructure.csproj ./backend.Infrastructure/
COPY backend.Application/backend.Application.csproj ./backend.Application/

RUN dotnet restore backend.API/backend.API.csproj

COPY . .

RUN dotnet publish backend.API/backend.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "backend.API.dll"]
