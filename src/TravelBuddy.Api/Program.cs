using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Neo4j.Driver;
// Uses the main module namespace for User, IUserRepository, IUserService, etc.
using TravelBuddy.Api.Auth; 
using TravelBuddy.Api.Factories; 
using TravelBuddy.Users; 
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Trips; 
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Messaging; 
using TravelBuddy.Messaging.Infrastructure;
using TravelBuddy.SharedKernel; 
using TravelBuddy.SharedKernel.Infrastructure;
using System.Text.Json;

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

builder.Services.AddSwaggerGen(options =>
{
    // Sets up the basic documentation information for the API
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelBuddy API", Version = "v1" });
    
    // Adds a custom filter to every single API endpoint
    // to include documentation for the 'X-Database-Type' header.
    // This allows users to select the database directly in the Swagger UI.
    options.OperationFilter<AddDatabaseHeaderParameter>(); 
});

// --- GET CONNECTION STRINGS TO THE DATABASES ---
// Get the connection strings
var mysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ;
    //?? throw new InvalidOperationException("MySQL connection string not found.");

var mongoDbConnectionString = builder.Configuration["ConnectionStrings:MongoDbConnection"] ;
    //?? throw new InvalidOperationException("MongoDB connection string not found.");
    
var neo4jUri = builder.Configuration["ConnectionStrings:Neo4jUri"];
    //?? throw new InvalidOperationException("Neo4j URI not found.");
var neo4jUser = builder.Configuration["ConnectionStrings:Neo4jUser"] ;
    //?? throw new InvalidOperationException("Neo4j User not found.");
var neo4jPassword = builder.Configuration["ConnectionStrings:Neo4jPassword"] ;
    //?? throw new InvalidOperationException("Neo4j Password not found.");

// --- MODULE CONFIGURATION ---
// 1. MySQL (EF Core) DbContexts Registration
// Register all DbContexts using the MySQL connection string
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseMySql(
        mysqlConnectionString, 
        Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(mysqlConnectionString), 
        b => b.MigrationsAssembly(typeof(UsersDbContext).Assembly.FullName)
    )
);
builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseMySql(
        mysqlConnectionString, 
        Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(mysqlConnectionString), 
        b => b.MigrationsAssembly(typeof(TripsDbContext).Assembly.FullName)
    )
);
builder.Services.AddDbContext<MessagingDbContext>(options =>
    options.UseMySql(
        mysqlConnectionString, 
        Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(mysqlConnectionString), 
        b => b.MigrationsAssembly(typeof(MessagingDbContext).Assembly.FullName)
    )
);
builder.Services.AddDbContext<SharedKernelDbContext>(options =>
    options.UseMySql(
        mysqlConnectionString, 
        Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(mysqlConnectionString), 
        b => b.MigrationsAssembly(typeof(SharedKernelDbContext).Assembly.FullName)
    )
);
// MongoDB Client Registration
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(mongoDbConnectionString));

// Neo4j Driver Registration
builder.Services.AddSingleton<IDriver>(s => 
    GraphDatabase.Driver(neo4jUri, AuthTokens.Basic(neo4jUser, neo4jPassword))
);

// --- Repository Registration ---
// Users Module
builder.Services.AddTransient<MySqlUserRepository>();
builder.Services.AddTransient<MongoDbUserRepository>();
builder.Services.AddTransient<Neo4jUserRepository>(); 

// Trips Module
builder.Services.AddTransient<MySqlTripRepository>();
builder.Services.AddTransient<MongoDbTripRepository>();
builder.Services.AddTransient<Neo4jTripRepository>();

// Messaging Module
builder.Services.AddTransient<MySqlMessagingRepository>();
builder.Services.AddTransient<MongoDbMessagingRepository>();
builder.Services.AddTransient<Neo4jMessagingRepository>();

// Register HTTP Context Accessor
// The factories need this to read the request header.
builder.Services.AddHttpContextAccessor();

// Register the FACTORIES themselves as Scoped
// Services will now depend on these interfaces.
builder.Services.AddScoped<IUserRepositoryFactory, UserRepositoryFactory>();
builder.Services.AddScoped<ITripRepositoryFactory, TripRepositoryFactory>();
builder.Services.AddScoped<IMessagingRepositoryFactory, MessagingRepositoryFactory>();

// --- Register the Services (Binds IUserService to UserService) ---
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();

builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

// Allow local dev frontend to access the API (used by the Vite dev server)
// We allow http://localhost:5173 as the dev frontend origin and enable credentials
// so cookies / auth flows can work when needed.
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// --- HTTP REQUEST PIPELINE CONFIGURATION ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// enable CORS for the dev frontend before authentication
app.UseCors("LocalDev");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers(); 

app.Run();