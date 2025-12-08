using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure.Security;
using Xunit;
using TravelBuddy.IntegrationTests.Helpers;

public class ChangePasswordTests : BaseIntegrationTest
{
    public ChangePasswordTests(TravelBuddyApiFactory<Program> factory) : base(factory) { }
    // ------------------------------------------------ Positive EP Tests ------------------------------------------------
    // Based on EP-CP2-New and EP-CP4-Old. DT-CP4
    [Theory]
    [InlineData("Password1234567!", "1234567Password!")] // new password length = 16, old password length = 16
    [InlineData("Password1234😁567!", "1234567Pass😒word!")] // Edge case - With emojis
    public async Task ChangePassword_WithValidData_ReturnsSuccess(string newPass, string oldPass)
    {
        var email = "user@test.com";
        int userId  = 1;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Test User", 
                PasswordHash = PasswordHasher.HashPassword(oldPass),
                Birthdate = new DateOnly(1990, 1, 1),
                Role = "user"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        await _client.AuthenticateAsync(email, oldPass);

        var dto = new ChangePasswordRequestDto
        {
            OldPassword = oldPass,
            NewPassword = newPass
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", dto);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    // ------------------------------------------------ Negative EP Tests ------------------------------------------------
    // Based on DT-CP1 - User does not exist (admin changes non-existent user's password)
    [Theory]
    [InlineData("Password123!", "NewPassword123!")]
    public async Task ChangePassword_AdminChangesNonExistentUserPassword_ReturnsFailure(string oldPassword, string newPassword)
    {
        var email = "user@test.com";
        int userId  = 1;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Test User", 
                PasswordHash = PasswordHasher.HashPassword(oldPassword),
                Birthdate = new DateOnly(1990, 1, 1),
                Role = "admin"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        await _client.AuthenticateAsync(email, oldPassword);

        var dto = new ChangePasswordRequestDto
        { 
            NewPassword = newPassword,
            OldPassword = oldPassword
        };

        var response = await _client.PatchAsJsonAsync("/api/users/111100/change-password", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    // Based on DT-CP1 - User does not exist (User gets deleted after login)
    [Fact]
    public async Task ChangePassword_WhenUserIsDeletedAfterLogin_ReturnsNotFound()
    {
        var email = "ghost@test.com";
        var password = "Password123!";
        int userId  = 1;

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "To Be Ghost", 
                PasswordHash = PasswordHasher.HashPassword(password), 
                Birthdate = new DateOnly(1990, 1, 1),
                Role = "user"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        await _client.AuthenticateAsync(email, password);

        // Manually delete the user from the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = db.Users.Find(userId);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        var dto = new ChangePasswordRequestDto 
        { 
            OldPassword = password,
            NewPassword = "NewPassword123!"
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
    }
    // Based on DT-CP2 - User is soft deleted
    [Theory]
    [InlineData("unique@test.com", "Password123!", "NewPassword123!")]
    public async Task ChangePassword_WithDeletedUser_ReturnsFailure(string email, string oldPassword, string newPassword)
    {
        int userId  = 1;
        // Manually Insert the User into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(oldPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user",
                IsDeleted = true
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var dto = new ChangePasswordRequestDto
        { 
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    // Based on DT-CP3 - Incorrect password
    [Theory]
    [InlineData("unique@test.com", "Password123", "Password123!")]
    public async Task ChangePassword_WithIncorrectPassword_ReturnsFailure(string email, string oldPassword, string newPassword)
    {
        int userId  = 1;
        // Manually Insert the User into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email,
                Name = "Existing User",
                PasswordHash = PasswordHasher.HashPassword(oldPassword + "extra"), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var dto = new ChangePasswordRequestDto
        { 
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    // ------------------------------------------------ Boundary Value Tests ------------------------------------------------
    [Theory]
    [InlineData(3, false)]    // EP-CP3-Old: below min length -> Should Fail
    [InlineData(5, false)]    // BV-P1: One below limit -> Should Fail
    [InlineData(6, true)]     // BV-P2: Min allowed length -> Should Succeed
    [InlineData(7, true)]     // BV-P3: One above limit -> Should Succeed
    public async Task ChangePassword_Enforces_OldPasswordLength_Boundary(int length, bool expectedSuccess)
    {
        // Create a string of 'A's with the exact length
        var validPassword = new string('A', 6);
        var oldVariableLength = new string('A', length);
        var newPassword = new string('B', 6);
        var email = "uniqueemail@test.com";

        int userId  = 1;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var passwordToHash = expectedSuccess ? oldVariableLength : validPassword;
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(passwordToHash), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        if (expectedSuccess)
        {
            await _client.AuthenticateAsync(email, oldVariableLength);
        } else {
            await _client.AuthenticateAsync(email, validPassword);
        }
        
        var ChangePasswordDto = new ChangePasswordRequestDto
        { 
            OldPassword = oldVariableLength,
            NewPassword = newPassword
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", ChangePasswordDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    [Theory]
    [InlineData(3, false)]    // EP-CP1-New: below min length -> Should Fail
    [InlineData(5, false)]    // BV-P1: One below limit -> Should Fail
    [InlineData(6, true)]     // BV-P2: Min allowed length -> Should Succeed
    [InlineData(7, true)]     // BV-P3: One above limit -> Should Succeed
    public async Task ChangePassword_Enforces_NewPasswordLength_Boundary(int length, bool expectedSuccess)
    {
        // Create a string of 'A's with the exact length
        var validPassword = new string('A', 6);
        var newPassword = new string('B', length);
        var email = "uniqueemail@test.com";

        int userId  = 1;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(validPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }


        await _client.AuthenticateAsync(email, validPassword);
        
        var ChangePasswordDto = new ChangePasswordRequestDto
        { 
            OldPassword = validPassword,
            NewPassword = newPassword
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", ChangePasswordDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    // ------------------------------------------------ Edge cases ------------------------------------------------
    [Theory]
    [InlineData(3, false)]
    [InlineData(5, false)]
    [InlineData(6, false)]
    [InlineData(7, false)]
    public async Task ChangePassword_WithWhitespacePassword_Fails(int length, bool expectedSuccess)
    {
        // Create a string of 'A's with the exact length
        var validPassword = new string('A', 6);
        var newPassword = new string(' ', length);
        var email = "uniqueemail@test.com";

        int userId  = 1;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(validPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }


        await _client.AuthenticateAsync(email, validPassword);
        
        var ChangePasswordDto = new ChangePasswordRequestDto
        { 
            OldPassword = validPassword,
            NewPassword = newPassword
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", ChangePasswordDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}