FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Server", "./Server"] 
COPY ["Shared", "./Shared"] 

RUN dotnet restore "./Server/MyWebAPI/MyWebAPI.csproj"
WORKDIR "/src/Server/MyWebAPI"
RUN dotnet build "MyWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebAPI.dll"]

# docker build --rm -f "Server/MyWebAPI/Dockerfile" -t mywebapi:latest .
# docker run --rm -d mywebapi:latest -p 8080:80
# az acr build --image samples/mywebapi:latest --registry myDemos --file Server/MyWebAPI/Dockerfile .
# az container create -g cloud-services --name mywebapi-cloud --image samples/mywebapi:latest --ip-address public --ports 443
# az container create -g cloud-services --name mywebapi-cloud --image mydemos.azurecr.io/samples/mywebapi:latest  --ip-address public --ports 443 80 --environment-variables ASPNETCORE_HTTPS_PORT=443 --registry-password <password>
#
# Documentation links: 
# https://docs.microsoft.com/en-us/cli/azure/container?view=azure-cli-latest#az-container-create
# https://docs.microsoft.com/en-us/azure/container-registry/container-registry-get-started-azure-cli