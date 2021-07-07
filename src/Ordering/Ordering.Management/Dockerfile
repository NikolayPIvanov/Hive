# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# copy and publish app and libraries
COPY ["src/Ordering/", "./Ordering/"]
COPY ["src/Identity/Hive.Identity.Contracts/", "./Identity/Hive.Identity.Contracts/"]
COPY ["src/GigManagement/Gig.Contracts/", "./GigManagement/Gig.Contracts/"]
COPY ["src/Billing/Billing.Contracts/", "./Billing/Billing.Contracts/"]

COPY ["src/Common/Common.Core/", "./Common/Common.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/", "./BuildingBlocks/BuildingBlocks.Core/"]

RUN dotnet publish "./Ordering/Ordering.Management/Ordering.Management.csproj" -c release -o out 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://*:5041
EXPOSE 5041

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Ordering.Management.dll"]