# Testing Approach in TravelBuddy

## Overview

In the TravelBuddy project, we use **unit testing with mocks** for the `Users` module rather than building a complete Docker-based test environment.  
This approach allows us to focus on **testing business logic** efficiently and reliably, while integration testing can later be performed with real services and databases.

---

## Unit Testing vs. Integration Testing

| Aspect | Unit Testing | Integration Testing |
|---------|---------------|---------------------|
| **Goal** | Test individual components (e.g., `UserService`) in isolation | Test interactions between components (e.g., `UserService` + Repository + Database) |
| **Dependencies** | Replaced with mocks or fakes | Real systems (DB, APIs, etc.) |
| **Speed** | Very fast (no I/O or setup) | Slower due to database, network, or container startup |
| **Reliability** | Deterministic – always gives same result | Can vary due to environment or data |
| **Tools** | xUnit + Moq | Docker Compose, real DBs, Postman, etc. |
| **Example** | Test that `AuthenticateAsync()` returns `null` for a non-existing user | Test that login works correctly against a running MySQL instance |

---

## Why We Use Mocks in Unit Tests

The **`UserService`** depends on data repositories that normally interact with MySQL, MongoDB, or Neo4j.  
For unit testing, we replace these dependencies with **mocks** created using [Moq](https://github.com/moq/moq4).  

### Benefits of Mocking

- ✅ **Isolation:** Tests focus purely on `UserService` logic, not database behavior.  
- ✅ **Speed:** No need to start containers or query a live database.  
- ✅ **Determinism:** Tests always behave the same, independent of network or data changes.  
- ✅ **Simplicity:** Easier to automate and run locally or in CI/CD.

### Example

```csharp
_userRepoMock
    .Setup(r => r.GetByEmailAsync("nouser@mail.com"))
    .ReturnsAsync((User?)null);

var result = await _userService.AuthenticateAsync("nouser@mail.com", "anypass");
Assert.Null(result);
```

## Mocking Approach: Classic (Detroit) vs. London (Mockist)

In software testing, there are two main schools of thought for using mocks:  
**Classic (Detroit)** and **London (Mockist)**.

| Aspect | Classic / Detroit | London / Mockist | Our Approach |
|--------|-------------------|------------------|---------------|
| **Focus** | Object state and returned results | Interactions and method calls | ✅ Classic |
| **Mocks used for** | External boundaries (DB, APIs) | Almost every collaborator | ✅ Only repositories |
| **Assertions** | Based on outputs (`Assert.Equal`, `Assert.Null`) | Based on calls (`Verify(...)`) | ✅ Output-based |
| **Example** | “Login returns null if user not found” | “Repo.GetUser was called once with this email” | ✅ Result-based |

### Which We Use: **Classic (Detroit)**

We follow the **Classic (Detroit)** approach to mocking and testing:

- We only mock **external dependencies** like the repository layer.  
- We assert on the **results and business logic output**, not on internal method calls.  
- We do **not** verify how often or in which order repository methods are called.  
- Our tests are focused on **what the system does**, not **how it does it**.

This approach makes the tests:

- ✅ More robust to refactoring  
- ✅ Easier to read and maintain  
- ✅ Aligned with our black-box test design (EP/BVA), which also focuses on observable results