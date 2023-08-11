# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80 443


# Copy Solution File to support Multi-Project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY TalentConsulting.TalentSuite.Reports.API.sln ./

# Copy Dependencies
COPY ["src/TalentConsulting.TalentSuite.Clients.API/TalentConsulting.TalentSuite.Clients.API.csproj", "src/TalentConsulting.TalentSuite.Clients.API/"]
COPY ["src/TalentConsulting.TalentSuite.Clients.Core/TalentConsulting.TalentSuite.Clients.Core.csproj", "src/TalentConsulting.TalentSuite.Clients.Core/"]
COPY ["src/TalentConsulting.TalentSuite.Clients.Infrastructure/TalentConsulting.TalentSuite.Clients.Infrastructure.csproj", "src/TalentConsulting.TalentSuite.Clients.Infrastructure.Infrastructure/"]
COPY ["src/TalentConsulting.TalentSuite.Clients.Common/TalentConsulting.TalentSuite.Clients.csproj", "src/TalentConsulting.TalentSuite.Clients.Common/"]

# Restore Project
RUN dotnet restore "src/TalentConsulting.TalentSuite.Clients.API/TalentConsulting.TalentSuite.Clients.API.csproj"

# Copy Everything
COPY . .

# Build
WORKDIR "/src/src/TalentConsulting.TalentSuite.Clients.API"
RUN dotnet build "TalentConsulting.TalentSuite.Clients.API.csproj" -c Release -o /app/build

# publish
FROM build AS publish
WORKDIR "/src/src/TalentConsulting.TalentSuite.Clients.API"
RUN dotnet publish "TalentConsulting.TalentSuite.Clients.API.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalentConsulting.TalentSuite.Clients.API.dll"]