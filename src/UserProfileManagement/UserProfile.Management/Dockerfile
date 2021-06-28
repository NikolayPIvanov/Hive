# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
EXPOSE 5001

# copy and publish app and libraries
COPY ["src/UserProfileManagement/", "./UserProfileManagement/"]
COPY ["src/Identity/Hive.Identity.Contracts/", "./Identity/Hive.Identity.Contracts/"]

COPY ["src/Common/Common.Core/", "./Common/Common.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/", "./BuildingBlocks/BuildingBlocks.Core/"]

RUN dotnet publish "./UserProfileManagement/UserProfile.Management/UserProfile.Management.csproj" -c release -o out 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "UserProfile.Management.dll"]