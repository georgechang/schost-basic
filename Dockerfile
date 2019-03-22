FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app

COPY src/GC.Plugin.Messaging ./GC.Plugin.Messaging
RUN dotnet restore ./GC.Plugin.Messaging/GC.Plugin.Messaging.csproj

COPY src/ScHost ./ScHost
RUN dotnet restore ./ScHost/ScHost.csproj

RUN dotnet publish -c Release -o out ./ScHost/ScHost.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS runtime
WORKDIR /app

COPY --from=build ./app/ScHost/out/ ./
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "ScHost.dll", "web"]