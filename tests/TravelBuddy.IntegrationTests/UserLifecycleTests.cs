using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using TravelBuddy.IntegrationTests.Helpers;
using Xunit;

[Collection("Integration Tests")]
public class UserLifecycleTests : BaseIntegrationTest
{
    public UserLifecycleTests(TravelBuddyApiFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task UserLifecycle_CompleteFlow_Register_Delete_LoginFails()
    {
        // -------------------------------------------------------
        // STATE 1: NONEXISTENT -> ACTIVE (Register)
        // -------------------------------------------------------
        var email = "lifecycle@test.com";
        var password = "Password123!";
        
        var registerDto = new RegisterRequestDto 
        { 
            Email = email, 
            Password = password, 
            Name = "Life Cycle User",
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var regResponse = await _client.PostAsJsonAsync("/api/users/register", registerDto);
        regResponse.StatusCode.Should().Be(HttpStatusCode.Created); // State is now ACTIVE

        // Get the ID for the next steps
        int userId;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = db.Users.First(u => u.Email == email);
            userId = user.UserId;
            
            // Verify DB State
            user.IsDeleted.Should().BeFalse(); 
        }

        // -------------------------------------------------------
        // STATE 2: ACTIVE -> DELETED (DeleteUser)
        // -------------------------------------------------------
        // We must login first to have permission to delete ourselves
        await _client.AuthenticateAsync(email, password);

        var delResponse = await _client.DeleteAsync($"/api/users/{userId}");
        delResponse.StatusCode.Should().Be(HttpStatusCode.NoContent); // State is now DELETED

        // Verify DB State
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var user = db.Users.AsNoTracking().FirstOrDefault(u => u.UserId == userId);
            
            // This matches your table: "User is soft-deleted"
            user.Should().NotBeNull("because the user should still exist in DB (soft-delete)");
            user!.IsDeleted.Should().BeTrue("because the state should transition to Deleted");
        }

        // -------------------------------------------------------
        // STATE 3: DELETED -> AUTHENTICATE (Fail)
        // -------------------------------------------------------
        // Try to login again with the correct password
        var loginResponse = await _client.PostAsJsonAsync("/api/users/login", new 
        { 
            Email = email, 
            Password = password 
        });

        // This matches your table: "Login always fails for deleted users"
        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UserLifecycle_PasswordChange_InvalidatesOldCredentials()
    {
        // 1. SETUP: Register a new user
        var email = "security_check@test.com";
        var oldPass = "OldPassword123!";
        var newPass = "NewPassword123!";

        await _client.PostAsJsonAsync("/api/users/register", new RegisterRequestDto 
        { 
            Email = email, 
            Password = oldPass, 
            Name = "Secure User",
            Birthdate = new DateOnly(2000, 1, 1)
        });

        // 2. STATE: Active. Login to get the cookie/id
        await _client.AuthenticateAsync(email, oldPass);

        // Get ID from DB (since we need it for the URL)
        int userId;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            userId = db.Users.First(u => u.Email == email).UserId;
        }

        // 3. TRANSITION: Active -> Active (Change Password)
        var changeResponse = await _client.PatchAsJsonAsync($"/api/users/{userId}/change-password", new ChangePasswordRequestDto
        {
            OldPassword = oldPass,
            NewPassword = newPass
        });
        changeResponse.EnsureSuccessStatusCode();

        // 4. VERIFY: Old Password should now FAIL
        var failLogin = await _client.PostAsJsonAsync("/api/users/login", new 
        { 
            Email = email, 
            Password = oldPass 
        });
        failLogin.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // 5. VERIFY: New Password should SUCCEED
        var successLogin = await _client.PostAsJsonAsync("/api/users/login", new 
        { 
            Email = email, 
            Password = newPass 
        });
        successLogin.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}