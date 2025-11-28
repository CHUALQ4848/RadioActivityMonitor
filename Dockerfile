# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY src/RadioactivityMonitor.Core/RadioactivityMonitor.Core.csproj ./src/RadioactivityMonitor.Core/
COPY tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj ./tests/RadioactivityMonitor.Tests/

# Restore dependencies
RUN dotnet restore src/RadioactivityMonitor.Core/RadioactivityMonitor.Core.csproj
RUN dotnet restore tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj

# Copy all source code
COPY src/ ./src/
COPY tests/ ./tests/

# Re-restore dependencies to ensure all packages are available
RUN dotnet restore tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj --force

# Build the solution
RUN dotnet build src/RadioactivityMonitor.Core/RadioactivityMonitor.Core.csproj -c Release --no-restore
RUN dotnet build tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj -c Release --no-restore

# Stage 2: Test
FROM build AS test
WORKDIR /app

# Run tests
RUN dotnet test tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj --no-build -c Release --logger "trx;LogFileName=test_results.trx" --results-directory /app/test-results

# For now, we'll use the test stage as default
FROM test AS final
WORKDIR /app

# Set entrypoint to run tests
ENTRYPOINT ["dotnet", "test", "tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj", "--no-build", "-c", "Release"]
