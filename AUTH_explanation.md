# Authentication and authorization

## Setup

### 1. Getting it ready for implementation
1.1 Added role column to user table
```mysql
role ENUM('user', 'admin') DEFAULT 'user'
```

1.2 Downloaded JWT to TravelBuddy.Api

Inside TravelBuddy.Api:
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### 2. Configure JWT

#### 2.1 In `Program.cs` inside TravelBuddy.Api

2.1.1 Insert this before builder.Build():
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
```

2.1.1 Then update the pipeline section:
```csharp
app.UseAuthentication(); // Add this before app.UseAuthorization();
```

#### 2.2 Inside `appsettings.json`

```json
"Jwt": {
  "Key": "YourSuperSecretKeyHere",
  "Issuer": "TravelBuddyAPI",
  "Audience": "TravelBuddyUsers"
}
```

> Note: Change the key

### 3. Adding TokenGenerator // TODO add namespace and update this

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelBuddy.Users.Models;

public class JwtTokenGenerator
{
    private readonly IConfiguration _config;

    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role) // Add role column to DB
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

Service added to `Program.cs`:
```csharp
builder.Services.AddScoped<JwtTokenGenerator>();
```

### Next steps

1. Make login endpoint
2. Create password hasher
    - Download `dotnet add package Microsoft.AspNetCore.Cryptography.KeyDerivation` to Modules/TravelBuddy.Users
3. Add `[Authorize]` before HttpGet line on protected endpoints