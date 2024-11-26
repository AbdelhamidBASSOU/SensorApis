using Microsoft.EntityFrameworkCore;
using SensorApis.Data;
using Microsoft.AspNetCore.Identity;
using SensorApis.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add In-Memory Cache
builder.Services.AddMemoryCache();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Include API version in response headers
    options.AssumeDefaultVersionWhenUnspecified = true; // Use default version if not specified
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default to version 1.0
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Ensure URL segment versioning
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<SensorDbContext>()
.AddDefaultTokenProviders();

// Register DbContext with PostgreSQL
builder.Services.AddDbContext<SensorDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Swagger/OpenAPI services with ConflictingActionsResolver
builder.Services.AddSwaggerGen(c =>
{
    // Add Swagger documentation for version 1
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sensor APIs",
        Version = "v1"
    });

    // Add Swagger documentation for version 2
    c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sensor APIs",
        Version = "v2"
    });

    // Resolve conflicting actions for versioning (ensure no conflicts in API paths)
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure middleware order
if (app.Environment.IsDevelopment())
{
    // Enable Swagger and SwaggerUI in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Specify Swagger JSON endpoints for versions 1 and 2
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sensor APIs V1");
        c.RoutePrefix = ""; // Set Swagger at the root URL
    });
}

// Uncomment for HTTPS Redirection if required
// app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Apply CORS policy before authentication
app.UseAuthentication();  // Enable authentication
app.UseAuthorization();   // Enable authorization

app.MapControllers();  // Map controller endpoints

app.Run();
