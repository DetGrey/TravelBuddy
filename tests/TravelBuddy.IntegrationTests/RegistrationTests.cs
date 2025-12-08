using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using Xunit;

public class RegistrationTests : BaseIntegrationTest
{
    public RegistrationTests(TravelBuddyApiFactory<Program> factory) : base(factory) { }
    // ------------------------------------------------ Positive EP Tests ------------------------------------------------
    // Based on EP-R2-Email, EP-R5-Name, EP-R8-Password. DT-R3
    [Theory]
    [InlineData(70, 50, "Password1234567!")] // email length = 70, name length = 50, password length = 16
    public async Task Register_WithValidData_CreatesUser_And_ReturnsSuccess(int emailLength, int nameLength, string rawPassword)
    {
        var ending = "@test.com";
        var variableLength = new string('A', emailLength - ending.Length); // Subtracting length of "@test.com"
        var email = $"{variableLength}{ending}";

        var variableNameLength = new string('B', nameLength);
        var name = variableNameLength;

        var registerDto = new RegisterRequestDto
        { 
            Email = email, 
            Password = rawPassword, 
            Name = name,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", registerDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        authResponse.Should().NotBeNull();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var userInDb = db.Users.FirstOrDefault(u => u.Email == email);
            
            userInDb.Should().NotBeNull();
            userInDb.Name.Should().Be(name);

            userInDb.PasswordHash.Should().NotBe(rawPassword, "because passwords must be hashed before saving");
        }
    }
    // ------------------------------------------------ Negative EP Tests ------------------------------------------------
    // Based on DT-R1
    [Theory]
    [InlineData("unique@test.com", "Password123!", "ValidUser", "Email already in use")] 
    public async Task Register_WithoutUniqueEmail_ReturnsConflict(
        string email, 
        string password, 
        string name, 
        string expectedErrorFragment)
    {
        // Manually Insert the "Existing User" into the DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            db.Users.Add(new User
            { 
                Email = email, 
                Name = "Existing User", 
                PasswordHash = "ExistingHash",
                Birthdate = new DateOnly(1990,1,1),
                Role = "user"
            });
            await db.SaveChangesAsync();
        }

        var registerDto = new RegisterRequestDto
        { 
            Email = email, 
            Password = password,
            Name = name,
            Birthdate = new DateOnly(1990, 1, 1) 
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", registerDto);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        // Check the Error Message (The variable you were missing!)
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain(expectedErrorFragment);

        // Database Check
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            var userInDb = db.Users.Where(u => u.Email == email).ToList();
            userInDb.Count.Should().Be(1, "Only the original user should exist in the DB");
        }
    }
    // Based on DT-R2
    [Theory]
    [InlineData("", "Password123!", "ValidUser", "Email field is required")] 
    [InlineData("unique2@test.com", "", "ValidUser", "Password field is required")] 
    public async Task Register_WithInvalidData_ReturnsBadRequest(
        string email, 
        string password, 
        string name, 
        string expectedErrorFragment)
    {
        var registerDto = new RegisterRequestDto
        { 
            Email = email, 
            Password = password,
            Name = name,
            Birthdate = new DateOnly(1990, 1, 1) 
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", registerDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Check the Error Message (The variable you were missing!)
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain(expectedErrorFragment);

        // Database Check
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            var userInDb = db.Users.FirstOrDefault(u => u.Email == email);
            userInDb.Should().BeNull("because validation should prevent DB saves");
        }
    }

    // ------------------------------------------------ Boundary Value Tests ------------------------------------------------
    [Theory]
    [InlineData(0, false)]    // EP-R4-Name: Empty string -> Should Fail
    [InlineData(99, true)]    // BV-N1: One below limit -> Should Succeed
    [InlineData(100, true)]   // BV-N2: Max allowed length -> Should Succeed
    [InlineData(101, false)]  // BV-N3: One over limit -> Should Fail
    [InlineData(150, false)]  // EP-R6-Name: Far over limit -> Should Fail
    public async Task Register_Enforces_NameLength_Boundary(int length, bool expectedSuccess)
    {
        // Create a string of 'A's with the exact length
        var variableLengthName = new string('A', length);
        
        var registerDto = new RegisterRequestDto
        { 
            Email = $"length{length}@test.com", 
            Password = "Password123!", 
            Name = variableLengthName,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", registerDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        // Database Check
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            var userInDb = db.Users.FirstOrDefault(u => u.Email == registerDto.Email);
            if (expectedSuccess)
            {
                userInDb.Should().NotBeNull("because registration should succeed");
            }
            else
            {
                userInDb.Should().BeNull("because registration should fail due to name length");
            }
        }
    }
    [Theory]
    [InlineData(3, false)]    // EP-R1-Email: below min length -> Should Fail
    [InlineData(5, false)]    // BV-E1: One below limit -> Should Fail
    [InlineData(6, true)]     // BV-E2: Min allowed length -> Should Succeed
    [InlineData(7, true)]     // BV-E3: One above limit -> Should Succeed
    [InlineData(149, true)]   // BV-E4: One below limit -> Should Succeed
    [InlineData(150, true)]   // BV-E5: Max allowed length -> Should Succeed
    [InlineData(151, false)]  // BV-E6: One over limit -> Should Fail
    [InlineData(200, false)]  // EP-R3-Email: well above max length -> Should Fail
    public async Task Register_Enforces_EmailLength_Boundary(int length, bool expectedSuccess)
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

        var registerDto = new RegisterRequestDto
        { 
            Email = email, 
            Password = password, 
            Name = "ValidUser",
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", registerDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        // Database Check
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            var userInDb = db.Users.FirstOrDefault(u => u.Email == registerDto.Email);
            if (expectedSuccess)
            {
                userInDb.Should().NotBeNull("because registration should succeed");
            }
            else
            {
                userInDb.Should().BeNull("because registration should fail due to email length");
            }
        }
    }
    [Theory]
    [InlineData(3, false)]    // EP-R7-Password: below min length -> Should Fail
    [InlineData(5, false)]    // BV-P1: One below limit -> Should Fail
    [InlineData(6, true)]     // BV-P2: Min allowed length -> Should Succeed
    [InlineData(7, true)]     // BV-P3: One above limit -> Should Succeed
    public async Task Register_Enforces_PasswordLength_Boundary(int length, bool expectedSuccess)
    {
        // Create a string of 'A's with the exact length
        var variableLength = new string('A', length);
        var email = $"uniqueemail@test.com";
        
        var registerDto = new RegisterRequestDto
        { 
            Email = email, 
            Name = "ValidUser",
            Password = variableLength,
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", registerDto);

        if (expectedSuccess)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    // ------------------------------------------------ Edge cases ------------------------------------------------
    // Case Insensitivity Collision
    [Theory]
    [InlineData("uniqueemail@test.com", "UniqueEmail@Test.com")]
    public async Task Register_WithEmail_CaseInsensitivityCollision_ReturnsConflict(string lowercaseEmail, string uppercaseEmail)
    {
        var rawPassword = "Password123!";
        var lowerRegisterDto = new RegisterRequestDto
        { 
            Email = lowercaseEmail, 
            Password = rawPassword, 
            Name = "lowercase user",
            Birthdate = new DateOnly(1990, 1, 1)
        };
        var UpperRegisterDto = new RegisterRequestDto
        { 
            Email = uppercaseEmail, 
            Password = rawPassword, 
            Name = "uppercase user",
            Birthdate = new DateOnly(1990, 1, 1)
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", lowerRegisterDto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        authResponse.Should().NotBeNull();

        var conflictResponse = await _client.PostAsJsonAsync("/api/users/register", UpperRegisterDto);
        conflictResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var userInDb = db.Users.FirstOrDefault(u => u.Email == lowercaseEmail);
           
            userInDb.Should().NotBeNull();
            userInDb.Name.Should().Be("lowercase user");

             var uppercaseUserInDb = db.Users.FirstOrDefault(u => u.Email == uppercaseEmail);
            // uppercase should be null or show lowercase user only
            uppercaseUserInDb.Should().BeNull("because email registration should be case-insensitive and prevent duplicates");
        }
    }

}