using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure.Security;
using Xunit;

public class LoginTests : BaseIntegrationTest
{
    public LoginTests(TravelBuddyApiFactory<Program> factory) : base(factory) { }
    // ------------------------------------------------ Positive EP Tests ------------------------------------------------
    // Based on EP-L2-Email and EP-L5-Password. DT-L4
    [Theory]
    [InlineData(70, "Password1234567!")] // email length = 70, password length = 16
    public async Task Login_WithValidData_ReturnsSuccess(int length, string rawPassword)
    {
        var ending = "@test.com";
        var variableLength = new string('A', length - ending.Length); // Subtracting length of "@test.com"
        var email = $"{variableLength}{ending}";
        
        // Manually Insert the User into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            db.Users.Add(new User
            { 
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(rawPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            });
            await db.SaveChangesAsync();
        }

        var dto = new LoginRequestDto
        { 
            Email = email,
            Password = rawPassword 
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", dto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        authResponse.Should().NotBeNull();
    }
    // ------------------------------------------------ Negative EP Tests ------------------------------------------------
    // Based on DT-L1 - Email does not exist
    [Theory]
    [InlineData("unique@test.com", "Password123!")]
    public async Task Login_WithNonExistentEmail_ReturnsFailure(string email, string rawPassword)
    {
        var dto = new LoginRequestDto
        { 
            Email = email,
            Password = rawPassword 
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    // Based on DT-L2 - User is deleted
    [Theory]
    [InlineData("unique@test.com", "Password123!")]
    public async Task Login_WithDeletedUser_ReturnsFailure(string email, string rawPassword)
    {
        // Manually Insert the User into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            db.Users.Add(new User
            { 
                Email = email, 
                Name = "Existing User", 
                PasswordHash = PasswordHasher.HashPassword(rawPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user",
                IsDeleted = true
            });
            await db.SaveChangesAsync();
        }

        var dto = new LoginRequestDto
        { 
            Email = email,
            Password = rawPassword 
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    // Based on DT-L3 - Incorrect password
    [Theory]
    [InlineData("unique@test.com", "Password123", "Password123!")]
    public async Task Login_WithIncorrectPassword_ReturnsFailure(string email, string rawPassword, string storedPassword)
    {
        // Manually Insert the User into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            db.Users.Add(new User
            { 
                Email = email, 
                Name = "Existing User",
                PasswordHash = PasswordHasher.HashPassword(storedPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            });
            await db.SaveChangesAsync();
        }

        var dto = new LoginRequestDto
        { 
            Email = email,
            Password = rawPassword 
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    // ------------------------------------------------ Boundary Value Tests ------------------------------------------------
    [Theory]
    [InlineData(3, false)]    // EP-L1: below min length -> Should Fail
    [InlineData(5, false)]    // BV-E1: One below limit -> Should Fail
    [InlineData(6, true)]     // BV-E2: Min allowed length -> Should Succeed
    [InlineData(7, true)]     // BV-E3: One above limit -> Should Succeed
    [InlineData(149, true)]   // BV-E4: One below limit -> Should Succeed
    [InlineData(150, true)]   // BV-E5: Max allowed length -> Should Succeed
    [InlineData(151, false)]  // BV-E6: One over limit -> Should Fail
    [InlineData(200, false)]  // EP-L3: well above max length -> Should Fail
    public async Task Login_Enforces_EmailLength_Boundary(int length, bool expectedSuccess)
    {
        // Create an email with exactly the specified length
        string email;
        if (length < 6)
        {
            // For lengths < 6, create invalid email (no @ domain)
            email = new string('a', length);
        }
        else if (length <= 150)
        {
            int aCount = length - 5; // -5 for "@b.co"
            email = $"{new string('a', aCount)}@b.co";
        }
        else
        {
            // For lengths > 150, create email that exceeds max
            int aCount = length - 8; // -8 for "@test.co"
            email = $"{new string('a', aCount)}@test.co";
        }
        var password = "Password123!";

        // Only insert valid emails into the database
        if (expectedSuccess)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
                
                db.Users.Add(new User
                { 
                    Email = email, 
                    Name = "Existing User", 
                    PasswordHash = PasswordHasher.HashPassword(password), 
                    Birthdate = new DateOnly(1990,1,1),
                    Role = "user"
                });
                await db.SaveChangesAsync();
            }
        }
        
        var loginDto = new LoginRequestDto
        { 
            Email = email, 
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", loginDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    [Theory]
    [InlineData(3, false)]    // EP-L4: below min length -> Should Fail
    [InlineData(5, false)]    // BV-P1: One below limit -> Should Fail
    [InlineData(6, true)]     // BV-P2: Min allowed length -> Should Succeed
    [InlineData(7, true)]     // BV-P3: One above limit -> Should Succeed
    public async Task Login_Enforces_PasswordLength_Boundary(int length, bool expectedSuccess)
    {
        // Create a string of 'A's with the exact length
        var variableLength = new string('A', length);
        var email = $"uniqueemail@test.com";

        // Only insert valid passwords into the DB
        if (expectedSuccess)
        {
            // Manually Insert the User into the DB
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
                
                db.Users.Add(new User
                { 
                    Email = email, 
                    Name = "Existing User", 
                    PasswordHash = PasswordHasher.HashPassword(variableLength), 
                    Birthdate = new DateOnly(1990,1,1),
                    Role = "user"
                });
                await db.SaveChangesAsync();
            }
        }
        
        var loginDto = new LoginRequestDto
        { 
            Email = email, 
            Password = variableLength
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", loginDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    // ------------------------------------------------ Edge Cases ------------------------------------------------
    [Fact]
    public async Task Login_WithEmojiInEmailAndPassword_CreatesUser_And_ReturnsSuccess()
    {
        var email = "emojiuser🤣❤️@test.com";
        var rawPassword = "Password✅123!";
        // Manually Insert the User into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            db.Users.Add(new User
            { 
                Email = email, 
                Name = "Existing User",
                PasswordHash = PasswordHasher.HashPassword(rawPassword), 
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            });
            await db.SaveChangesAsync();
        }

        var loginDto = new LoginRequestDto
        {
            Email = email,
            Password = rawPassword
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        authResponse.Should().NotBeNull();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var userInDb = db.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            
            userInDb.Should().NotBeNull();
            userInDb.Name.Should().Be("Existing User");
        }
    }
}