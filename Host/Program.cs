using CareerPath.Assessment.Core;
using CareerPath.Assessment.Infrastructure;
using CareerPath.Identity.Core;
using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.Entities;
using CareerPath.Identity.Infrastructure;
using CareerPath.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using CareerPath.Careers.Core;
using CareerPath.Careers.Infrastructure;
using CareerPath.Careers.Infrastructure.Persistence;
using CareerPath.Profiles.Core;
using CareerPath.Profiles.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRATION PHASE ---
// Wire up the Identity module's infrastructure (Database, DI, Identity Core)

// Standard API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- Update your AddSwaggerGen block to this ---
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
// Add Identity Module Dependencies
builder.Services.AddIdentityCoreServices();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
// Add Assessment Module Dependencies
builder.Services.AddAssessmentCore();
builder.Services.AddAssessmentInfrastructure(builder.Configuration);
// Add Careers Module Dependencis 
builder.Services.AddCareersCore();
builder.Services.AddCareersInfrastructure(builder.Configuration);
// Register Global Exception Handling
builder.Services.AddExceptionHandler<CareerPath.Host.Middleware.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Register the Profiles Module Dependencies
builder.Services.AddProfilesCore();
builder.Services.AddProfilesInfrastructure(builder.Configuration);

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

        // Execute identity seeding 
        await IdentityDataSeeder.SeedAsync(userManager, roleManager, configuration);

        // Execute Careers Seeding
        var careersSeeder = scopedProvider.GetRequiredService<CareersDataSeeder>();
        await careersSeeder.SeedAsync();
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