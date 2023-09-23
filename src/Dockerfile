# Use the official Microsoft .NET 8 RC1 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0.100-rc.1-jammy-amd64 AS build-env

# Set the working directory
WORKDIR /app

# Copy csproj and restore dependencies
# Copy the solution file and restore the packages


# Copy everything else and build
COPY . ./src
RUN dotnet restore ./src/WebUI.Razor
RUN dotnet publish ./src/WebUI.Razor -c Release -o out

# Use the official Microsoft .NET 8 RC1 runtime image for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 80


# Set the entry point for the application
ENTRYPOINT ["dotnet", "WebUI.Razor.dll"]
