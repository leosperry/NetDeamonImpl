FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY NetDaemonApps/*.csproj ./NetDaemonApps/
COPY MyHome/MyHome.DI/*.csproj ./MyHome/MyHome.DI/
COPY MyHome/MyHome.Interfaces/*.csproj ./MyHome/MyHome.Interfaces/
COPY MyHome/MyHome.Logic/*.csproj ./MyHome/MyHome.Logic/
COPY MyHome/MyHome.Tests/*.csproj ./MyHome/MyHome.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out ./NetDaemonApps/NetDaemonApps.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NetDaemonApps.dll"]
