# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ["src/Chat/Hive.Chat/Hive.Chat.csproj", "./Chat/Hive.Chat/Hive.Chat.csproj"]
COPY ["src/Identity/Hive.Identity/Hive.Identity.csproj", "./Identity/Hive.Identity/Hive.Identity.csproj"]
COPY ["src/Common/Common.Core/Common.Core.csproj", "./Common/Common.Core/Common.Core.csproj"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj", "./BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj"]
RUN dotnet restore "./Chat/Hive.Chat/Hive.Chat.csproj"

# copy and publish app and libraries
COPY ["src/Chat/Hive.Chat/", "./Chat/Hive.Chat/"]
COPY ["src/Identity/Hive.Identity.Contracts/", "./Identity/Hive.Identity.Contracts/"]

COPY ["src/Common/Common.Core/", "./Common/Common.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/", "./BuildingBlocks/BuildingBlocks.Core/"]

RUN dotnet publish "./Chat/Hive.Chat/Hive.Chat.csproj" -c release -o out 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://*:6001
EXPOSE 6001

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Hive.Chat.dll"]