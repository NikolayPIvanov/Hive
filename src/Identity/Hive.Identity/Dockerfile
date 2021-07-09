# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ["src/Identity/Hive.Identity.Contracts/Hive.Identity.Contracts.csproj", "./Identity/Hive.Identity.Contracts/Hive.Identity.Contracts.csproj"]
COPY ["src/Identity/Hive.Identity/Hive.Identity.csproj", "./Identity/Hive.Identity/Hive.Identity.csproj"]
COPY ["src/Common/Common.Core/Common.Core.csproj", "./Common/Common.Core/Common.Core.csproj"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj", "./BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj"]
RUN dotnet restore "./Identity/Hive.Identity/Hive.Identity.csproj"

# copy and publish app and libraries
COPY ["src/Identity/Hive.Identity/", "./Identity/Hive.Identity/"]
COPY ["src/Identity/Hive.Identity.Contracts/", "./Identity/Hive.Identity.Contracts/"]

COPY ["src/Common/Common.Core/", "./Common/Common.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/", "./BuildingBlocks/BuildingBlocks.Core/"]
RUN dotnet publish "./Identity/Hive.Identity/Hive.Identity.csproj" -o out 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

ENV ASPNETCORE_URLS="https://+:7001;http://+:7000"
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password="mypass123"

EXPOSE 7001
EXPOSE 7000

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Hive.Identity.dll"]