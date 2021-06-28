# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
EXPOSE 5057

# copy and publish app and libraries
COPY ["src/GigManagement/", "./GigManagement/"]
COPY ["src/Identity/Hive.Identity.Contracts/", "./Identity/Hive.Identity.Contracts/"]
COPY ["src/Investing/Investing.Contracts/", "./Investing/Investing.Contracts/"]
COPY ["src/Ordering/Ordering.Contracts/", "./Ordering/Ordering.Contracts/"]

COPY ["src/Common/Common.Core/", "./Common/Common.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/", "./BuildingBlocks/BuildingBlocks.Core/"]

RUN dotnet publish "./GigManagement/Gig.Management/Gig.Management.csproj" -c release -o out 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Gig.Management.dll"]