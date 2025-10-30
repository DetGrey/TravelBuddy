using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
// Uses the main module namespace for User, IUserRepository, IUserService, etc.
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
// NEW: This registers the services needed for your Controllers (UsersController)
builder.Services.AddControllers(); 
// NEW: Authorization service registration (the fix from the last step)
builder.Services.AddAuthorization();

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

var app = builder.Build();

// --- 2. HTTP REQUEST PIPELINE CONFIGURATION ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); 
app.MapControllers(); 

app.Run();