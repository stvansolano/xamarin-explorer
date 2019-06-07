FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Server/MyWebAPI/MyWebAPI.csproj Server/MyWebAPI/

COPY Shared/Shared.csproj Shared/
RUN dotnet restore Server/MyWebAPI/MyWebAPI.csproj

COPY . .
WORKDIR /src/Server/MyWebAPI
RUN dotnet build MyWebAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MyWebAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyWebAPI.dll"]
EXPOSE 80
# docker build --rm -f "Server/MyWebAPI/Dockerfile" -t mywebapi:latest .
# docker run --rm -d mywebapi:latest -p 8080:80