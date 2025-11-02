using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// Uses the main module namespace for User, IUserRepository, IUserService, etc.
using TravelBuddy.Api.Auth; 
using TravelBuddy.Users; 
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Trips; 
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Messaging; 
using TravelBuddy.Messaging.Infrastructure;
using TravelBuddy.SharedKernel; 
using TravelBuddy.SharedKernel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICE CONFIGURATION ---

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// This registers the services needed for your Controllers (UsersController)
builder.Services.AddControllers();
// Authorization service registration
builder.Services.AddAuthorization();

// Add JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT secret key is not configured.");
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

         // Extract token from cookie if Authorization header is missing
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            }
        };
    });


// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// --- MODULE CONFIGURATION ---
// Register the Users DbContext
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(UsersDbContext).Assembly.FullName)
    )
);

// Register the Trips DbContext
builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(TripsDbContext).Assembly.FullName)
    )
);

// Register the Messaging DbContext
builder.Services.AddDbContext<MessagingDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(MessagingDbContext).Assembly.FullName)
    )
);

// Register the SharedKernel DbContext
builder.Services.AddDbContext<SharedKernelDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(SharedKernelDbContext).Assembly.FullName)
    )
);

// 1. Register the Repository (Binds IUserRepository to UserRepository)
builder.Services.AddScoped<IUserRepository, UserRepository>();
// 2. Register the Service (Binds IUserService to UserService)
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITripDestinationRepository, TripDestinationRepository>();
builder.Services.AddScoped<ITripDestinationService, TripDestinationService>();

builder.Services.AddScoped<IBuddyRepository, BuddyRepository>();
builder.Services.AddScoped<IBuddyService, BuddyService>();

builder.Services.AddScoped<JwtTokenGenerator>();

var app = builder.Build();

// --- 2. HTTP REQUEST PIPELINE CONFIGURATION ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers(); 

app.Run();