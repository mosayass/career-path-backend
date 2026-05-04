using CareerPath.Assessment.Core;
using CareerPath.Assessment.Infrastructure;
using CareerPath.Careers.Core;
using CareerPath.Careers.Infrastructure;
using CareerPath.Careers.Infrastructure.Persistence;
using CareerPath.Community.Core;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Infrastructure;
using CareerPath.Community.Infrastructure.Persistence;
using CareerPath.Identity.Core;
using CareerPath.Identity.Core.Entities;
using CareerPath.Identity.Infrastructure;
using CareerPath.Identity.Infrastructure.Persistence;
using CareerPath.Profiles.Core;
using CareerPath.Profiles.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CareerPath.Assessment.Infrastructure.Persistence;
using CareerPath.Profiles.Infrastructure.Persistence;


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

// Add Community Module Dependencies
builder.Services.AddCommunityCore();
builder.Services.AddCommunityInfrastructure(builder.Configuration);

//Add CORS for frontend connection in development
var developmentCorsPolicy = "_developmentCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: developmentCorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:3003")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Required if you are sending cookies or auth headers
        });
    // Add this new production policy
    options.AddPolicy(name: "productionCorsPolicy", policy =>
    {
        // Pulls the real link dynamically from environment variables
        var frontendUrl = builder.Configuration["FrontendUrl"];

        if (!string.IsNullOrEmpty(frontendUrl))
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});
var app = builder.Build();

// --- 2. EXECUTION PHASE (Data Seeding) ---
// Create a temporary service scope to run our seeder exactly once on startup
using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        // DELETE On developement 
        // 1. Apply Migrations for all modules first 
        // (Replace with your actual DbContext class names for each module)
        var identityContext = scopedProvider.GetRequiredService<IdentityDbContext>();
        await identityContext.Database.MigrateAsync();

        var careersContext = scopedProvider.GetRequiredService<CareersDbContext>();
        await careersContext.Database.MigrateAsync();

        var assessmentContext = scopedProvider.GetRequiredService<AssessmentsDbContext>();
        await assessmentContext.Database.MigrateAsync();

        var profilesContext = scopedProvider.GetRequiredService<ProfilesDbContext>();
        await profilesContext.Database.MigrateAsync();

        var communityContext = scopedProvider.GetRequiredService<CommunityDbContext>();
        await communityContext.Database.MigrateAsync();

        // Request the required services from the DI Container
        var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<Role>>();
        var configuration = scopedProvider.GetRequiredService<IConfiguration>();

        // Execute identity seeding 
        await IdentityDataSeeder.SeedAsync(userManager, roleManager, configuration);

        // Execute Careers Seeding
        var careersSeeder = scopedProvider.GetRequiredService<CareersDataSeeder>();
        await careersSeeder.SeedAsync();

        // Execute Community Seeding
        var communitySeeder = scopedProvider.GetRequiredService<CommunityDataSeeder>();
        await communitySeeder.SeedAsync();

        // Initialize CORS rules for Azure Blob Storage to allow frontend uploads
        var storageService = scopedProvider.GetRequiredService<IStorageService>();
        await storageService.InitializeCorsRulesAsync(CancellationToken.None);
    }
    catch (Exception ex)
    {
        // Safely catch and log any seeding errors without crashing the whole app
        var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// --- HTTP PIPELINE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(developmentCorsPolicy);
}
else
{
    app.UseCors("productionCorsPolicy");
}

app.UseExceptionHandler();
// Ensure ASP.NET Core knows to use routing and auth
app.UseRouting();
app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();