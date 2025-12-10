# CI/CD Pipeline Test Output Documentation

## Overview
This document provides example outputs from the continuous integration pipeline that automatically runs unit tests and integration tests for the TravelBuddy application.

---

## Pipeline Configuration

**File Location**: `.github/workflows/ci.yml`

**Triggers**:
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop` branches
- Manual workflow dispatch

**Jobs**:
1. **unit-tests**: Runs all unit tests
2. **integration-tests**: Runs integration tests with Testcontainers MySQL
3. **test-report**: Generates comprehensive test reports

**Note**: Integration tests use **Testcontainers** to automatically spin up isolated MySQL Docker containers. No external database services are required in the CI pipeline.

---

## CI Pipeline Execution Summary

### Workflow Run Example

**Workflow**: CI/CD Pipeline - TravelBuddy  
**Run #**: 42  
**Triggered by**: push to main  
**Commit**: abc1234 - "Add integration tests for user lifecycle"  
**Duration**: 3m 45s  
**Status**: ✅ Success  

#### Job Breakdown

| Job                | Status | Duration |
|--------------------|--------|----------|
| unit-tests         | ✅ Pass | 1m 12s   |
| integration-tests  | ✅ Pass | 2m 08s   |
| test-report        | ✅ Pass | 25s      |

#### Test Statistics

```
Total Tests Run: 11
├── Unit Tests: 5
│   ├── Passed: 5
│   ├── Failed: 0
│   └── Skipped: 0
└── Integration Tests: 6
    ├── Passed: 6
    ├── Failed: 0
    └── Skipped: 0

Overall Status: ✅ ALL TESTS PASSED
Code Coverage: 81.4% (Target: 80%)
```

---

## Artifacts Generated

The CI pipeline automatically generates and uploads the following artifacts:

1. **unit-test-results**: TRX format test result files
2. **unit-test-coverage**: Cobertura XML coverage reports
3. **integration-test-results**: TRX format test result files
4. **integration-test-coverage**: Cobertura XML coverage reports

These artifacts are available for download for 90 days after the workflow run completes.

---

## Running Tests Locally

### Unit Tests Only
```bash
dotnet test tests/TravelBuddy.UnitTests/TravelBuddy.Tests.csproj --verbosity normal
```

### Integration Tests Only
```bash
# Testcontainers will automatically start MySQL - no manual setup needed!
dotnet test tests/TravelBuddy.IntegrationTests/TravelBuddy.IntegrationTests.csproj --verbosity normal
```

### All Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

---

## Troubleshooting

### Common Issues

**Issue**: Docker is not running  
**Solution**: Ensure Docker Desktop is running before executing integration tests

**Issue**: Tests fail with "Cannot connect to Docker daemon"  
**Solution**: Start Docker service and verify with `docker ps`

**Issue**: Testcontainers timeout on container startup  
**Solution**: Check Docker resources (CPU/Memory) and network connectivity

**Issue**: Test discovery fails  
**Solution**: Ensure test projects reference Microsoft.NET.Test.Sdk
