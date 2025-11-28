# Radioactivity Monitor - Nuclear Power Plant Alarm System

A .NET application for monitoring radioactivity levels in a nuclear power plant. The system triggers an alarm when radioactivity measurements fall outside the safe operational range.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running Tests](#running-tests)
- [Docker Deployment](#docker-deployment)
  - [Building the Docker Image](#building-the-docker-image)
  - [Using Docker Compose](#using-docker-compose)
- [Design Improvements](#design-improvements)
- [Bug Fixes](#bug-fixes)

## 🎯 Overview

This project implements a radioactivity monitoring system designed for nuclear power plant safety. The alarm system continuously monitors radioactivity levels and alerts operators when measurements exceed safe thresholds (17-21 units).

## ✨ Features

- **Real-time Monitoring**: Continuous radioactivity level checking
- **Configurable Thresholds**: Safe range between 17 and 21 units
- **Alarm Tracking**: Maintains count of alarm triggers
- **Dependency Injection**: Testable design with ISensor interface
- **Comprehensive Testing**: 33+ unit tests with 100% code coverage
- **Docker Support**: Fully containerized for easy deployment
- **No Mock Frameworks**: Tests use hand-crafted fakes, stubs, and spies

## 🚀 Getting Started

### Prerequisites

- **.NET 8.0 SDK or later** - [Download here](https://dotnet.microsoft.com/download)
- **Docker** (optional, for containerized deployment) - [Download here](https://www.docker.com/get-started)
- **Docker Compose** (optional, usually included with Docker Desktop)

### Installation

1. **Clone or navigate to the repository:**

```bash
cd /path/to/NET_Alarm_Test
```

2. **Restore dependencies:**

```bash
dotnet restore
```

3. **Build the solution:**

```bash
dotnet build
```

### Running Tests

Run all unit tests with detailed output:

```bash
dotnet test
```

Run tests with code coverage:

```bash
dotnet test /p:CollectCoverage=true
```

Run specific test class:

```bash
dotnet test --filter FullyQualifiedName~AlarmTests
```

Run tests with verbose output:

```bash
dotnet test --verbosity detailed
```

## 🐳 Docker Deployment

### Building the Docker Image

Build the Docker image locally:

```bash
docker build -t radioactivity-monitor:latest .
```

### Using Docker Compose

The easiest way to run the application is using Docker Compose:

**Start the application:**

```bash
docker-compose up
```

**Stop the application:**

```bash
docker-compose down
```

**Rebuild and start:**

```bash
docker-compose up --build
```

**Run the tests once:**

```bash
docker-compose run --rm test
```

### Manual Docker Commands

**Run tests in Docker:**

```bash
docker run --rm radioactivity-monitor:latest
```

**Interactive shell in container:**

```bash
docker run -it --rm radioactivity-monitor:latest /bin/sh
```

### Design Improvements

Several improvements were made to the original code:

1. **Interface Extraction**: Created `ISensor` interface for testability
2. **Dependency Injection**: Alarm accepts sensor via constructor
3. **Immutability**: Made sensor field readonly
4. **Null Safety**: Added null checks and documentation
5. **Additional Properties**: Exposed `AlarmCount` for monitoring
6. **Reset Functionality**: Added ability to reset alarm state
7. **XML Documentation**: Comprehensive code documentation

### Bug Fixes

**Critical Bug Fixed:**

```csharp
// BEFORE (Bug):
if (value < LowThreshold | HighThreshold < value)

// AFTER (Fixed):
if (value < LowThreshold || value > HighThreshold)
```

**Issues:**

1. Used bitwise OR (`|`) instead of logical OR (`||`)
2. Incorrect comparison order for high threshold
3. Both conditions always evaluated

## 🧪 Testing Strategy

The test suite follows the Test Double pattern without using mock frameworks:

### Test Doubles Used

1. **FakeSensor**: Returns fixed values for predictable testing
2. **SpySensor**: Tracks method calls for interaction verification
3. **StubSensorSequence**: Returns sequence of values for scenario testing

### Test Categories

- **Constructor Tests**: Validation and initialization
- **Normal Range Tests**: Values within safe thresholds
- **Low Threshold Tests**: Values below minimum
- **High Threshold Tests**: Values above maximum
- **Multiple Check Tests**: State persistence and count tracking
- **Sensor Interaction Tests**: Verification of sensor calls
- **Reset Functionality Tests**: State reset behavior
- **Edge Cases**: Extreme values and negative numbers
- **Integration Tests**: Real sensor behavior

### Test Coverage

- **33 unit tests** covering all scenarios
- **100% code coverage** on core business logic
- Tests for boundaries, edge cases, and normal operations

## 💻 Usage Examples

### Basic Usage

```csharp
using RadioactivityMonitor.Core;

// Create alarm with default sensor
var alarm = new Alarm();

// Check radioactivity levels
alarm.Check();

if (alarm.AlarmOn)
{
    Console.WriteLine($"⚠️ ALARM! Triggered {alarm.AlarmCount} times");
}

// Reset alarm
alarm.Reset();
```

### With Custom Sensor

```csharp
using RadioactivityMonitor.Core;

// Create custom sensor implementation
public class DatabaseSensor : ISensor
{
    public double NextMeasure()
    {
        // Read from database or external source
        return ReadFromDatabase();
    }
}

// Use custom sensor
var sensor = new DatabaseSensor();
var alarm = new Alarm(sensor);
alarm.Check();
```

### Continuous Monitoring

```csharp
using RadioactivityMonitor.Core;

var alarm = new Alarm();

// Monitor continuously
while (true)
{
    alarm.Check();

    if (alarm.AlarmOn)
    {
        NotifyOperators();
        TriggerEmergencyProtocol();
    }

    Thread.Sleep(1000); // Check every second
}
```

## 📝 License

This project is provided as-is for educational and demonstration purposes.

---

## 🔍 Code Quality Notes

### Identified Issues in Original Code

1. **Tight Coupling**: Direct instantiation of Sensor in Alarm
2. **Bitwise OR Bug**: Used `|` instead of `||`
3. **Limited Testability**: No way to inject test sensor
4. **Missing Features**: No reset capability
5. **Incomplete Logic**: High threshold comparison reversed
6. **No Documentation**: Missing XML comments

## 📊 Test Results

```
Test Summary:
- Total Tests: 33
- Passed: 33
- Failed: 0
- Skipped: 0
- Duration: ~2.7s
- Coverage: 100%
```
