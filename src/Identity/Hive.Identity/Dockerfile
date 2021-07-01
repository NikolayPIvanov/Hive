# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# copy and publish app and libraries
COPY ["src/Identity/Hive.Identity/", "./Identity/Hive.Identity/"]
COPY ["src/Identity/Hive.Identity.Contracts/", "./Identity/Hive.Identity.Contracts/"]

COPY ["src/Common/Common.Core/", "./Common/Common.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/", "./BuildingBlocks/BuildingBlocks.Core/"]
RUN dotnet publish "./Identity/Hive.Identity/Hive.Identity.csproj" -c release -o out 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://*:7001
EXPOSE 7001

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Hive.Identity.dll"]