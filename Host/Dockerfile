# 1. Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
RUN apt-get update && apt-get install -y libgssapi-krb5-2
USER app
WORKDIR /app
EXPOSE 8080

# 2. Build image with the SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy everything from the root into the Docker build context
COPY . .

# Restore and build the Host project
WORKDIR "/src/Host"
RUN dotnet restore "CareerPath.Host.csproj"
RUN dotnet build "CareerPath.Host.csproj" -c Release -o /app/build

# 3. Publish the application
FROM build AS publish
RUN dotnet publish "CareerPath.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false -m:1

# 4. Final production-ready image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CareerPath.Host.dll"]