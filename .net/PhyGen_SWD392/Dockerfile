# ===== STAGE 1: Build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY . ./

# Restore packages
RUN dotnet restore "./PhyGen_SWD392.csproj"

# Build and publish
RUN dotnet publish "./PhyGen_SWD392.csproj" -c Release -o /app/publish

# ===== STAGE 2: Run =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose default port
EXPOSE 80

# Entry point
ENTRYPOINT ["dotnet", "PhyGen_SWD392.dll"]
