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

    // ----------------- AuthenticateAsync (Login) -----------------

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

    // ----------------- RegisterAsync -----------------

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


    // ----------------- ChangePasswordAsync -----------------

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

    // ----------------- DeleteUser -----------------

    [Fact]
    public async Task DeleteUser_ShouldReturnResultFromRepository_WhenDeleteSucceeds()
    {
        // Arrange
        const int userId = 42;
        _userRepoMock
            .Setup(r => r.DeleteAsync(userId, It.IsAny<string>()))
            .ReturnsAsync((true, (string?)null));

        // Act
        var (success, error) = await _userService.DeleteUser(userId);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        _userRepoMock.Verify(r => r.DeleteAsync(userId, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        const int userId = 42;
        _userRepoMock
            .Setup(r => r.DeleteAsync(userId, It.IsAny<string>()))
            .ReturnsAsync((false, "already deleted"));

        // Act
        var (success, error) = await _userService.DeleteUser(userId);

        // Assert
        Assert.False(success);
        Assert.Equal("already deleted", error);
        _userRepoMock.Verify(r => r.DeleteAsync(userId, It.IsAny<string>()), Times.Once);
    }

    // ----------------- GetUserByIdAsync -----------------

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
    private const string ValidPassword = "validPwd"; 
    private const string TooShortPassword = "12345";
    private const string MinLengthPassword = "123456";
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

    [Fact]
    public void Login_EmailTooShort_FailsValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(5),
            Password = ValidPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginRequestDto.Email)));
    }

    [Fact]
    public void Login_EmailWithinValidRange_PassesValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(6),
            Password = ValidPassword
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    [Fact]
    public void Login_EmailTooLong_FailsValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(151),
            Password = ValidPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginRequestDto.Email)));
    }

    [Fact]
    public void Login_PasswordTooShort_FailsValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(10),
            Password = TooShortPassword
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(LoginRequestDto.Password)));
    }

    [Fact]
    public void Login_PasswordValid_PassesValidation()
    {
        var dto = new LoginRequestDto
        {
            Email = MakeEmailOfLength(10),
            Password = MinLengthPassword
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // ----- RegisterRequestDto -----

    [Fact]
    public void Register_EmailTooShort_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(5),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Email)));
    }

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

    [Fact]
    public void Register_EmailTooLong_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(151),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Email)));
    }

    [Fact]
    public void Register_NameEmpty_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "",
            Email = MakeEmailOfLength(10),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));
    }

    [Fact]
    public void Register_NameValidLength_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = new string('a', 100),
            Email = MakeEmailOfLength(10),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    [Fact]
    public void Register_NameTooLong_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = new string('a', 101),
            Email = MakeEmailOfLength(10),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));
    }

    [Fact]
    public void Register_PasswordTooShort_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(10),
            Password = TooShortPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Password)));
    }

    [Fact]
    public void Register_PasswordValid_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(10),
            Password = MinLengthPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

        [Fact]
    public void Register_NameWhitespaceOnly_FailsValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "   ", // only spaces
            Email = MakeEmailOfLength(10),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequestDto.Name)));
    }

        [Fact]
    public void Register_NameWithEmoji_PassesValidation()
    {
        var dto = new RegisterRequestDto
        {
            Name = "User 🤡",
            Email = MakeEmailOfLength(10),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }


    // ----- ChangePasswordRequestDto -----

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

    [Fact]
    public void ChangePassword_BothPasswordsValid_PassesValidation()
    {
        var dto = new ChangePasswordRequestDto
        {
            OldPassword = MinLengthPassword,
            NewPassword = "abcdef"
        };

        var results = Validate(dto);

        Assert.Empty(results);
    }

    // ----- Boundary values for email length -----

    [Theory]
    [InlineData(5, true)]   // too short
    [InlineData(6, false)]  // min ok
    [InlineData(150, false)]// max ok
    [InlineData(151, true)] // too long
    public void Register_EmailBoundaryValues_AreValidated(int length, bool expectError)
    {
        var dto = new RegisterRequestDto
        {
            Name = "User",
            Email = MakeEmailOfLength(length),
            Password = ValidPassword,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var results = Validate(dto);

        if (expectError)
        {
            Assert.NotEmpty(results);
        }
        else
        {
            Assert.Empty(results);
        }
    }

    // ----- Boundary values for password length -----

    [Theory]
    [InlineData(5, true)]  // too short
    [InlineData(6, false)] // minimum
    [InlineData(7, false)] // above minimum
    public void PasswordBoundaryValues_AreValidated(int length, bool expectError)
    {
        var dto = new ChangePasswordRequestDto
        {
            OldPassword = new string('a', length),
            NewPassword = ValidPassword
        };

        var results = Validate(dto);

        if (expectError)
        {
            Assert.NotEmpty(results);
        }
        else
        {
            Assert.Empty(results);
        }
    }
}