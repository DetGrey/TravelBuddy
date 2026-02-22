using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure.Security;

namespace TravelBuddy.Tests;

public class UserServiceTests
{
    private const string DefaultUserEmail = "user@mail.com";
    private const string DefaultUserName = "Test User";
    private const string DefaultPassword = "secret1";
    private const string DefaultOldPassword = "oldSecret";
    private const string TooShortPassword = "12345";

    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>(MockBehavior.Strict);

        // Repository factory is only needed here, so keep it local (Sonar S1450)
        var factoryMock = new Mock<IUserRepositoryFactory>(MockBehavior.Strict);
        factoryMock
            .Setup(f => f.GetUserRepository())
            .Returns(_userRepoMock.Object);

        _userService = new UserService(factoryMock.Object);
    }

    // =====================================================
    // Decision Tables – AuthenticateAsync (Login)
    // =====================================================

    // DT-L1: Email does not exist -> Fail
    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        const string email = "nouser@mail.com";
        _userRepoMock
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.AuthenticateAsync(email, "anypass");

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
    }

    // DT-L2: Email exists, user deleted -> Fail
    // STD-DEL-AUTH: Deleted -> Authenticate -> Fail
    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User
        {
            UserId = 1,
            Email = DefaultUserEmail,
            Name = "Deleted User",
            PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
            Birthdate = new DateOnly(1990, 1, 1),
            IsDeleted = true,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(DefaultUserEmail))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.AuthenticateAsync(DefaultUserEmail, DefaultPassword);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(DefaultUserEmail), Times.Once);
    }

    // DT-L3: Email exists, not deleted, password incorrect -> Fail
    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        // Arrange
        var hashed = PasswordHasher.HashPassword(DefaultPassword);
        var user = new User
        {
            UserId = 2,
            Email = DefaultUserEmail,
            Name = "Active User",
            PasswordHash = hashed,
            Birthdate = new DateOnly(1990, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(DefaultUserEmail))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.AuthenticateAsync(DefaultUserEmail, "wrongpass");

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(DefaultUserEmail), Times.Once);
    }

    // DT-L4: Email exists, not deleted, password correct -> Success
    [Fact]
    public async Task Authenticate_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var hashed = PasswordHasher.HashPassword(DefaultPassword);
        var user = new User
        {
            UserId = 3,
            Email = DefaultUserEmail,
            Name = "Active User",
            PasswordHash = hashed,
            Birthdate = new DateOnly(1990, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(DefaultUserEmail))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.AuthenticateAsync(DefaultUserEmail, DefaultPassword);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.UserId, result!.UserId);
        Assert.Equal(user.Email, result.Email);
        _userRepoMock.Verify(r => r.GetByEmailAsync(DefaultUserEmail), Times.Once);
    }

    // =====================================================
    // Decision Tables – RegisterAsync
    // =====================================================


    // DT-R2: Email unique = Yes, missing required fields -> Fail (validation error)
    // Missing field: Email too short (< 6)
    [Fact]
    public async Task Register_ShouldReturnNull_WhenEmailTooShort()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "a@b.c", // length < 6
            Password = DefaultPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }


    // DT-R2: Email unique = Yes, missing required fields -> Fail (validation error)
    // Missing field: Password too short (< 6)
    [Fact]
    public async Task Register_ShouldReturnNull_WhenPasswordTooShort()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "valid@mail.com",
            Password = TooShortPassword, // < 6
            Birthdate = new DateOnly(1990, 1, 1)
        };

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    // DT-R2: Email unique = Yes, missing required fields -> Fail (validation error)
    // Missing field: Birthdate missing (null)
    [Fact]
    public async Task Register_ShouldReturnNull_WhenBirthdateMissing()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "valid@mail.com",
            Password = DefaultPassword,
            Birthdate = null
        };

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    // DT-R1: Email unique = No, required fields present -> Fail (email already in use)
    [Fact]
    public async Task Register_ShouldReturnNull_WhenEmailAlreadyInUse()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "duplicate@mail.com",
            Password = DefaultPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var existingUser = new User
        {
            UserId = 99,
            Email = request.Email,
            Name = "Existing",
            PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
            Birthdate = new DateOnly(1980, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(request.Email), Times.Once);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    // DT-R3: Email unique = Yes, all required fields valid -> Success (user created)
    // STD-REG-ACT: Nonexistent -> Active (Register success)
    [Fact]
    public async Task Register_ShouldCreateUser_WhenAllFieldsValidAndEmailUnique()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "newuser@mail.com",
            Password = DefaultPassword,
            Birthdate = new DateOnly(1995, 5, 5)
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        User? addedUser = null;
        _userRepoMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => addedUser = u)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Email, result!.Email);
        Assert.Equal(DefaultUserName, result.Name);
        Assert.Equal(request.Birthdate!.Value, result.Birthdate);
        Assert.Equal("user", result.Role);

        Assert.NotNull(addedUser);
        Assert.Equal(request.Email, addedUser!.Email);
        Assert.Equal(DefaultUserName, addedUser.Name);
        Assert.NotEqual(DefaultPassword, addedUser.PasswordHash); // must be hashed

        _userRepoMock.Verify(r => r.GetByEmailAsync(request.Email), Times.Once);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    // =====================================================
    // Edge Cases
    // =====================================================
    
    // Edge-DB1: Case-insensitive email collision    
    [Fact]
    public async Task Register_ShouldFail_WhenEmailDiffersOnlyByCase()
    {
        // Arrange
        const string existingEmailLower = "test@email.com";

        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "Test@Email.com", // same logical email, different case
            Password = DefaultPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var existingUser = new User
        {
            UserId = 123,
            Name = DefaultUserName,
            Email = existingEmailLower,
            PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
            Birthdate = new DateOnly(1985, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        // Simulate a case-insensitive repository/db by matching on lowercase
        _userRepoMock
            .Setup(r => r.GetByEmailAsync(It.Is<string>(e => e.ToLower() == existingEmailLower)))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    // Edge-CON1: Concurrent registration with same email
    [Fact]
    public async Task Register_TwoRequestsSameEmail_OneSucceedsOneFails()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = DefaultUserName,
            Email = "concurrent@mail.com",
            Password = DefaultPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var existingUser = new User
        {
            UserId = 999,
            Name = DefaultUserName,
            Email = request.Email,
            PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
            Birthdate = new DateOnly(1980, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        // First call → no user yet, second call → user already exists
        _userRepoMock
            .SetupSequence(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null)      // first registration attempt
            .ReturnsAsync(existingUser);    // second registration attempt

        _userRepoMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        var firstResult = await _userService.RegisterAsync(request);
        var secondResult = await _userService.RegisterAsync(request);

        // Assert
        Assert.NotNull(firstResult); // first registration succeeds
        Assert.Null(secondResult);   // second registration fails (email already in use)

        _userRepoMock.Verify(r => r.GetByEmailAsync(request.Email), Times.Exactly(2));
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }


    // =====================================================
    // Decision Tables – ChangePasswordAsync
    // =====================================================

    // DT-CP1: User exists = No -> Fail
    // STD-NON-CP: Nonexistent -> ChangePassword -> Fail (state unchanged)
    [Fact]
    public async Task ChangePassword_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        var request = new ChangePasswordRequestDto
        {
            OldPassword = DefaultOldPassword,
            NewPassword = DefaultPassword
        };

        _userRepoMock
            .Setup(r => r.GetUserByIdAsync(1))
            .ReturnsAsync((User?)null);

        // Act
        var (success, error) = await _userService.ChangePasswordAsync(request, 1);

        // Assert
        Assert.False(success);
        Assert.Equal("User not found or is deleted.", error);
        _userRepoMock.Verify(r => r.UpdatePasswordAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    // DT-CP2: User exists = Yes, IsDeleted = Yes -> Fail
    // STD-DEL-CP: Deleted -> ChangePassword -> Fail (state unchanged)
    [Fact]
    public async Task ChangePassword_ShouldFail_WhenUserIsDeleted()
    {
        // Arrange
        var request = new ChangePasswordRequestDto
        {
            OldPassword = DefaultOldPassword,
            NewPassword = DefaultPassword
        };

        var user = new User
        {
            UserId = 1,
            Email = "deleted@mail.com",
            Name = DefaultUserName,
            PasswordHash = PasswordHasher.HashPassword(DefaultOldPassword),
            Birthdate = new DateOnly(1990, 1, 1),
            IsDeleted = true,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetUserByIdAsync(1))
            .ReturnsAsync(user);

        // Act
        var (success, error) = await _userService.ChangePasswordAsync(request, 1);

        // Assert
        Assert.False(success);
        Assert.Equal("User not found or is deleted.", error);
        _userRepoMock.Verify(r => r.UpdatePasswordAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    // DT-CP3: User exists = Yes, IsDeleted = No, Old password correct = No -> Fail
    // STD-ACT-CP-FAIL: Active -> ChangePassword(fail) -> Active (no change)
    [Fact]
    public async Task ChangePassword_ShouldFail_WhenOldPasswordIncorrect()
    {
        // Arrange
        var request = new ChangePasswordRequestDto
        {
            OldPassword = "wrongOld",
            NewPassword = DefaultPassword
        };

        var user = new User
        {
            UserId = 1,
            Email = DefaultUserEmail,
            Name = DefaultUserName,
            PasswordHash = PasswordHasher.HashPassword(DefaultOldPassword),
            Birthdate = new DateOnly(1990, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetUserByIdAsync(1))
            .ReturnsAsync(user);

        // Act
        var (success, error) = await _userService.ChangePasswordAsync(request, 1);

        // Assert
        Assert.False(success);
        Assert.Equal("Old password is incorrect.", error);
        _userRepoMock.Verify(r => r.UpdatePasswordAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    // DT-CP4: User exists = Yes, IsDeleted = No, Old password correct = Yes -> Success
    // STD-ACT-CP-SUCCESS: Active -> ChangePassword(success) -> Active (password updated)
    [Fact]
    public async Task ChangePassword_ShouldSucceed_WhenUserAndOldPasswordValid()
    {
        // Arrange
        var request = new ChangePasswordRequestDto
        {
            OldPassword = DefaultOldPassword,
            NewPassword = DefaultPassword
        };

        var user = new User
        {
            UserId = 1,
            Email = DefaultUserEmail,
            Name = DefaultUserName,
            PasswordHash = PasswordHasher.HashPassword(DefaultOldPassword),
            Birthdate = new DateOnly(1990, 1, 1),
            IsDeleted = false,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetUserByIdAsync(1))
            .ReturnsAsync(user);

        _userRepoMock
            .Setup(r => r.UpdatePasswordAsync(1, It.IsAny<string>()))
            .ReturnsAsync((true, (string?)null));

        // Act
        var (success, error) = await _userService.ChangePasswordAsync(request, 1);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        _userRepoMock.Verify(r => r.UpdatePasswordAsync(1, It.IsAny<string>()), Times.Once);
    }

    // =====================================================
    // Decision Tables – DeleteUser
    // =====================================================

    // DT-DU1: User does not exist -> Fail - no user to delete
    [Fact]
    public async Task DeleteUser_DT_DU1_ShouldFail_WhenUserDoesNotExist()
    {
        // Arrange
        const int userId = 42;

        string? capturedHash = null;
        _userRepoMock
            .Setup(r => r.DeleteAsync(userId, It.IsAny<string>()))
            .Callback<int, string>((_, hash) => capturedHash = hash)
            .ReturnsAsync((false, "no user to delete"));

        // Act
        var (success, error) = await _userService.DeleteUser(userId);

        // Assert
        Assert.False(success);
        Assert.Equal("no user to delete", error);

        Assert.False(string.IsNullOrWhiteSpace(capturedHash)); // proves service passed something
        _userRepoMock.Verify(r => r.DeleteAsync(userId, It.IsAny<string>()), Times.Once);
    }

    // DT-DU2 (design intent): If repository reports "already deleted", service returns failure.
    // Note: MySQL delete_user procedure currently does not distinguish already-deleted users.

    [Fact]
    public async Task DeleteUser_DT_DU2_ShouldFail_WhenUserAlreadyDeleted()
    {
        // Arrange
        const int userId = 42;

        string? capturedHash = null;
        _userRepoMock
            .Setup(r => r.DeleteAsync(userId, It.IsAny<string>()))
            .Callback<int, string>((_, hash) => capturedHash = hash)
            .ReturnsAsync((false, "already deleted"));

        // Act
        var (success, error) = await _userService.DeleteUser(userId);

        // Assert
        Assert.False(success);
        Assert.Equal("already deleted", error);

        Assert.False(string.IsNullOrWhiteSpace(capturedHash));
        _userRepoMock.Verify(r => r.DeleteAsync(userId, It.IsAny<string>()), Times.Once);
    }

    // DT-DU3: User exists and not deleted -> Success - user marked deleted
    // STD-ACT-DEL: Active -> Deleted via DeleteUser
    [Fact]
    public async Task DeleteUser_DT_DU3_ShouldSucceed_WhenUserExistsAndNotDeleted()
    {
        // Arrange
        const int userId = 42;

        string? capturedHash = null;
        _userRepoMock
            .Setup(r => r.DeleteAsync(userId, It.IsAny<string>()))
            .Callback<int, string>((_, hash) => capturedHash = hash)
            .ReturnsAsync((true, (string?)null));

        // Act
        var (success, error) = await _userService.DeleteUser(userId);

        // Assert
        Assert.True(success);
        Assert.Null(error);

        Assert.False(string.IsNullOrWhiteSpace(capturedHash));
        _userRepoMock.Verify(r => r.DeleteAsync(userId, It.IsAny<string>()), Times.Once);
    }


    // ----------------- GetUserByIdAsync -----------------

    // DT-GET-1: User does not exist → null
    [Fact]
    public async Task GetUserById_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        _userRepoMock
            .Setup(r => r.GetUserByIdAsync(1))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.GetUserByIdAsync(1), Times.Once);
    }

    // DT-GET-2: User exists → mapped DTO returned
    [Fact]
    public async Task GetUserById_ShouldMapUserToDto_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            UserId = 10,
            Name = "User",
            Email = DefaultUserEmail,
            Birthdate = new DateOnly(1990, 1, 1),
            PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
            IsDeleted = false,
            Role = "user"
        };

        _userRepoMock
            .Setup(r => r.GetUserByIdAsync(10))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.UserId, result!.UserId);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Birthdate, result.Birthdate);
        _userRepoMock.Verify(r => r.GetUserByIdAsync(10), Times.Once);
    }

    // ----------------- GetAllUsersAsync -----------------
    // FR-GETALL-1: All users returned and mapped correctly
    [Fact]
    public async Task GetAllUsers_ShouldMapAllUsersToDtos()
    {
        // Arrange
        var users = new[]
        {
            new User
            {
                UserId = 1,
                Name = "User One",
                Email = "one@mail.com",
                Birthdate = new DateOnly(1990,1,1),
                PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
                IsDeleted = false,
                Role = "user"
            },
            new User
            {
                UserId = 2,
                Name = "User Two",
                Email = "two@mail.com",
                Birthdate = new DateOnly(1991,1,1),
                PasswordHash = PasswordHasher.HashPassword(DefaultPassword),
                IsDeleted = false,
                Role = "user"
            }
        };

        _userRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, u => u.UserId == 1 && u.Email == "one@mail.com");
        Assert.Contains(list, u => u.UserId == 2 && u.Email == "two@mail.com");
        _userRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    // ----------------- GetUserAuditsAsync -----------------
    // FR-AUDIT-1: User audits returned and mapped correctly
    [Fact]
    public async Task GetUserAudits_ShouldMapAuditsToDtos()
    {
        // Arrange
        var audits = new[]
        {
            new UserAudit
            {
                AuditId = 1,
                UserId = 10,
                Action = "CREATE",
                FieldChanged = null,
                OldValue = null,
                NewValue = null,
                ChangedBy = null,
                Timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new UserAudit
            {
                AuditId = 2,
                UserId = 10,
                Action = "DELETE",
                FieldChanged = "IsDeleted",
                OldValue = "false",
                NewValue = "true",
                ChangedBy = 99,
                Timestamp = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        _userRepoMock
            .Setup(r => r.GetUserAuditsAsync())
            .ReturnsAsync(audits);

        // Act
        var result = await _userService.GetUserAuditsAsync();

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, a => a.AuditId == 1 && a.Action == "CREATE");
        Assert.Contains(list, a => a.AuditId == 2 && a.Action == "DELETE");
        _userRepoMock.Verify(r => r.GetUserAuditsAsync(), Times.Once);
    }
}


// --------------------------------------------------------------------
// DTO Validation Tests (Equivalence & Boundary Values)
// --------------------------------------------------------------------

public class UserDtoValidationTests
{
    private const string ValidPassword = "aaaaaaaaaaaaaaaa"; // length 16 (EP valid)
    private const string TooShortPassword = "aaa";           // length 3 (EP invalid)
    private const string MinLengthPassword = "aaaaaa"; // also 6 (EP valid)

    private static IList<ValidationResult> Validate(object dto)
    {
        var ctx = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, ctx, results, validateAllProperties: true);
        return results;
    }

    private static string MakeEmailOfLength(int length)
    {
        const string domain = "@x.dk"; // 5 characters
        int localLength = length - domain.Length;
        if (localLength <= 0)
        {
            return new string('a', length); // still respects length
        }
        return new string('a', localLength) + domain;
    }

    // ----- LoginRequestDto -----

    // EP-L1: Email invalid length (0–5), representative = 3
    [Fact]
    public void Login_EmailTooShort_FailsValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(3),
            Password = ValidPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginRequestDto.Email)));
    }

    // EP-L2: Email valid length (6–150), representative = 70
    [Fact]
    public void Login_EmailWithinValidRange_PassesValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(70),
            Password = ValidPassword
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // EP-L3: Email invalid length (151+), representative = 200
    [Fact]
    public void Login_EmailTooLong_FailsValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(200),
            Password = ValidPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginRequestDto.Email)));
    }


    // EP-L4: Password invalid length (0–5), representative = 3
    [Fact]
    public void Login_PasswordTooShort_FailsValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(70),
            Password = TooShortPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginRequestDto.Password)));
    }

    // EP-L5: Password valid length (6+), representative = 6
    [Fact]
    public void Login_PasswordValid_PassesValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(70),
            Password = MinLengthPassword
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // ----- RegisterRequestDto -----

    // EP-R1: Email invalid length (0–5), representative = 3
    [Fact]
    public void Register_EmailTooShort_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(3),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Email)));
    }

    // EP-R2: Email valid length (6–150), representative = 70
    [Fact]
    public void Register_EmailValidLength_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // EP-R3: Email invalid length (151+), representative = 200
    [Fact]
    public void Register_EmailTooLong_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(200),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Email)));
    }

    // EP-R4: Name empty (length = 0)
    [Fact]
    public void Register_NameEmpty_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "",
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));
    }

    // EP-R5: Name valid length (1–100), representative = 50
    [Fact]
    public void Register_NameValidLength_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = new string('a', 50),
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // EP-R6: Name invalid length (101+), representative = 150
    [Fact]
    public void Register_NameTooLong_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = new string('a', 150),
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));
    }

    // EP-R7: Password invalid length (0–5), representative = 3
    [Fact]
    public void Register_PasswordTooShort_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(70),
            Password = TooShortPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Password)));
    }

    // EP-R8: Password valid length (6+), representative = 16
    [Fact]
    public void Register_PasswordValid_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // EP-R9: Name invalid – whitespace only
    [Fact]
    public void Register_NameWhitespaceOnly_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "   ", // only spaces
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));
    }

    // EP-R10: Name valid – non-alphanumeric characters allowed (emoji)
    [Fact]
    public void Register_NameWithEmoji_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User 🤡",
            Email = MakeEmailOfLength(70),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }


    // ----- ChangePasswordRequestDto -----

    // EP-CP3-Old: OldPassword invalid length (0–5), rep = 3
    [Fact]
    public void ChangePassword_OldPasswordTooShort_FailsValidation()
    {
        var dto = new ChangePasswordRequestDto
        {
            OldPassword = TooShortPassword,
            NewPassword = MinLengthPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ChangePasswordRequestDto.OldPassword)));
    }

    // EP-CP1-New: NewPassword invalid length (0–5), rep = 3
    [Fact]
    public void ChangePassword_NewPasswordTooShort_FailsValidation()
    {
        var dto = new ChangePasswordRequestDto
        {
            OldPassword = MinLengthPassword,
            NewPassword = TooShortPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ChangePasswordRequestDto.NewPassword)));
    }

    // EP-CP2-New + EP-CP4-Old: both passwords valid length (6+), rep = 16
    [Fact]
    public void ChangePassword_BothPasswordsValid_PassesValidation()
    {
        var dto = new ChangePasswordRequestDto
        {
            OldPassword = ValidPassword,
            NewPassword = ValidPassword
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // ----- Boundary Value Analysis (BVA) -----

    // ----- Boundary values for email length -----

    [Theory]
    [InlineData("BV-E1", 5, true)]
    [InlineData("BV-E2", 6, false)]
    [InlineData("BV-E3", 7, false)]
    [InlineData("BV-E4", 149, false)]
    [InlineData("BV-E5", 150, false)]
    [InlineData("BV-E6", 151, true)]
    public void Register_EmailBoundaryValues_AreValidated(
        string bvaId,
        int length, 
        bool expectError)
    {
        _ = bvaId;

        var dto = new RegisterRequestDto
        {
            Name = new string('a', 50),         // valid representative (matches your EP)
            Email = MakeEmailOfLength(length),
            Password = MinLengthPassword,       // valid (6) (BV-p2)
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        var hasEmailError = results.Any(r =>
            r.MemberNames.Contains(nameof(RegisterRequestDto.Email)));

        Assert.Equal(expectError, hasEmailError);

        // if email is expected to be OK, nothing else should fail either
        if (!expectError)
            Assert.Empty(results);
    }

    // ----- Boundary values for password length -----

    [Theory]
    [InlineData("BV-P1", 5, true)]
    [InlineData("BV-P2", 6, false)]
    [InlineData("BV-P3", 7, false)]
    public void ChangePassword_OldPasswordBoundaryValues_AreValidated(
        string bvaId, 
        int length, 
        bool expectError)
    {
        _ = bvaId;

        var dto = new ChangePasswordRequestDto
        {
            OldPassword = new string('a', length),
            NewPassword = MinLengthPassword // valid
        };

        var results = Validate(dto);
        var hasOldPwdError = results.Any(r => 
            r.MemberNames.Contains(nameof(ChangePasswordRequestDto.OldPassword)));

        Assert.Equal(expectError, hasOldPwdError);

        // if old password is expected OK, nothing should fail
        if (!expectError)
            Assert.Empty(results);
    }


    // ----- Boundary values for name length -----

    [Theory]
    [InlineData("BV-N1", 99, false)]
    [InlineData("BV-N2", 100, false)]
    [InlineData("BV-N3", 101, true)]
    public void Register_NameBoundaryValues_AreValidated(
        string bvaId, 
        int length, 
        bool expectError)
    {
        _ = bvaId;

        var dto = new RegisterRequestDto
        {
            Name = new string('a', length),
            Email = MakeEmailOfLength(70),
            Password = MinLengthPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);
        var hasNameError = results.Any(r => 
            r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));

        Assert.Equal(expectError, hasNameError);

        // if name is expected OK, nothing else should fail
        if (!expectError)
            Assert.Empty(results);
    }


}