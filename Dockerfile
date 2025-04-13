# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /CodeSparkRequestAPI
COPY ["codeSparkRequestAPI/codeSparkRequestAPI.csproj", "codeSparkRequestAPI/"]
RUN dotnet restore "codeSparkRequestAPI/codeSparkRequestAPI.csproj"
COPY . .
WORKDIR "/CodeSparkRequestAPI/codeSparkRequestAPI"
RUN dotnet build "codeSparkRequestAPI.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "codeSparkRequestAPI.csproj" -c Release -o /app/publish
EXPOSE 5001
# Final image setup
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "codeSparkRequestAPI.dll"]