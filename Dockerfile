# Use the official image from Microsoft
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . ./
RUN dotnet restore

# Build the app
RUN dotnet build --no-restore -c Release

# Publish the app
RUN dotnet publish --no-build -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build-env /app/out .

# Define the command to start the app
ENTRYPOINT ["dotnet", "CodeMatcherV2Api.dll", "--urls", "http://0.0.0.0:80"]
