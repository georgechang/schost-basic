FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app

COPY *.csproj ./ScHost/
COPY nuget.config ./ScHost/
RUN dotnet restore ./ScHost/ScHost.csproj

COPY . ./ScHost/
RUN dotnet publish -c Release -o out ./ScHost/ScHost.csproj

FROM mcr.microsoft.com/dotnet/core/runtime AS runtime
WORKDIR /app
#ADD bin/Debug/netcoreapp2.2/publish/* ./
COPY --from=build ./app/ScHost/out/ ./
ENTRYPOINT ["dotnet", "ScHost.dll"]