using CareerPath.Identity.Core.Entities;
using CareerPath.Identity.Infrastructure;
using CareerPath.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using CareerPath.Identity.Core;
using CareerPath.Identity.Core.Contracts;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRATION PHASE ---
// Wire up the Identity module's infrastructure (Database, DI, Identity Core)

// Standard API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Identity Module Dependencies
builder.Services.AddIdentityCoreServices();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Register Global Exception Handling
builder.Services.AddExceptionHandler<CareerPath.Host.Middleware.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// --- 2. EXECUTION PHASE (Data Seeding) ---
// Create a temporary service scope to run our seeder exactly once on startup
using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        // Request the required services from the DI Container
        var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<Role>>();
        var configuration = scopedProvider.GetRequiredService<IConfiguration>();
        var passwordHasher = scopedProvider.GetRequiredService<IPasswordHasher>(); // <-- ADD THIS LINE

        // Execute the seeding logic
        await IdentityDataSeeder.SeedAsync(userManager, roleManager, configuration, passwordHasher);
    }
    catch (Exception ex)
    {
        // Safely catch and log any seeding errors without crashing the whole app
        var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the Identity database.");
    }
}

// --- HTTP PIPELINE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
// Ensure ASP.NET Core knows to use routing and auth
app.UseRouting();
app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();