using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure.Security;
using TravelBuddy.IntegrationTests.Helpers;
using Xunit;

public class DeleteUserTests : BaseIntegrationTest
{
    public DeleteUserTests(TravelBuddyApiFactory<Program> factory) : base(factory) { }
    // ------------------------------------------------ Positive EP Tests ------------------------------------------------
    // DT-DU3
    [Fact]
    public async Task DeleteUser_WithValidData_ReturnsSuccess()
    {
        var email = "email@test.com";
        var rawPassword = "Password123!";
        int userId  = 1;

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            db.Users.Add(new User
            { 
                UserId = userId,
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(rawPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            });
            await db.SaveChangesAsync();
        }

        await _client.AuthenticateAsync(email, rawPassword);

        var response = await _client.DeleteAsync($"/api/users/{userId}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    // ------------------------------------------------ Negative EP Tests ------------------------------------------------
    // Based on DT-DU1 - User does not exist (User gets deleted after login)
    [Fact]
    public async Task DeleteUser_WhenUserIsDeletedAfterLogin_ReturnsNotFound()
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

        var response = await _client.DeleteAsync($"/api/users/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
    }
    // Based on DT-DU2 - User is soft deleted
    [Fact]
    public async Task DeleteUser_WithDeletedUser_ReturnsFailure()
    {
        var email = "deleteduser@test.com";
        var oldPassword = "Password123!";
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

        var response = await _client.DeleteAsync($"/api/users/{userId}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}