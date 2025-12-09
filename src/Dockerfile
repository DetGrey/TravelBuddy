# STAGE 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# 1. Copy the project file first to cache the "restore" step
# This makes future builds much faster if you haven't changed dependencies
COPY TravelBuddy.Api/TravelBuddy.Api.csproj TravelBuddy.Api/
RUN dotnet restore TravelBuddy.Api/TravelBuddy.Api.csproj

# 2. Copy the rest of the source code
COPY . .

# 3. Build and Publish the application
# We publish to a folder named /app inside the container
WORKDIR /source/TravelBuddy.Api
RUN dotnet publish -c Release -o /app

# STAGE 2: Run the application
# We use the lighter ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the built files from Stage 1
COPY --from=build /app .

# Configure for Render
# .NET 8 defaults to port 8080. Render will detect this automatically.
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# The name here must match your project name exactly
ENTRYPOINT ["dotnet", "TravelBuddy.Api.dll"]