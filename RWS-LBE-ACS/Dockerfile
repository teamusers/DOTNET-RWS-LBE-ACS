# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1) Copy only the .csproj and restore
COPY ["RWS-LBE-ACS.csproj", "./"]
RUN dotnet restore "RWS-LBE-ACS.csproj"

# 2) Copy the rest of your code and build
COPY . .
RUN dotnet build "RWS-LBE-ACS.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RWS-LBE-ACS.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# 3) Copy publish output + templates
COPY --from=publish /app/publish .
COPY templates ./templates

ENTRYPOINT ["dotnet","RWS-LBE-ACS.dll"]