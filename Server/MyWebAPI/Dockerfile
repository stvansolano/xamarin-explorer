FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Server/MyWebAPI/MyWebAPI.csproj", "Server/MyWebAPI/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Server/MyWebAPI/MyWebAPI.csproj"
COPY . .
WORKDIR "/src/Server/MyWebAPI"
RUN dotnet build "MyWebAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyWebAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyWebAPI.dll"]
EXPOSE 80
# docker build --rm -f "Server/MyWebAPI/Dockerfile" -t mywebapi:latest .
# docker run --rm -d mywebapi:latest -p 8080:80