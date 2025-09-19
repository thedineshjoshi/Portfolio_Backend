# Use the official ASP.NET runtime as a base image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY ["Portfolio_Backend.csproj", "./"]
RUN dotnet restore "./Portfolio_Backend.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "Portfolio_Backend.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Portfolio_Backend.dll"]
