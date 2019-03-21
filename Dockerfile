FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app

COPY src/GC.Plugin.Messaging/*.csproj ./ScHost/
COPY src/GC.Plugin.Messaging/nuget.config ./ScHost/
RUN dotnet restore ./ScHost/GC.Plugin.Messaging.csproj
COPY src/ ./GC.Plugin.Messaging/
RUN dotnet build ./ScHost/GC.Plugin.Messaging.csproj

COPY src/**/*.csproj ./ScHost/
COPY src/ScHost/nuget.config ./ScHost/
RUN dotnet restore ./ScHost/ScHost.csproj

COPY src/ ./ScHost/
RUN dotnet publish -c Release -o out ./ScHost/ScHost.csproj

FROM mcr.microsoft.com/dotnet/core/runtime AS runtime
WORKDIR /app
#ADD bin/Debug/netcoreapp2.2/publish/* ./
COPY --from=build ./app/ScHost/out/ ./
ENTRYPOINT ["dotnet", "ScHost.dll", "web"]