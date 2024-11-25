using Microsoft.EntityFrameworkCore;
using SensorApis.Data;
using Microsoft.AspNetCore.Identity;
using SensorApis.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

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

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<SensorDbContext>()
.AddDefaultTokenProviders();

// Register DbContext with PostgreSQL
builder.Services.AddDbContext<SensorDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware order
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Ensure Swagger uses HTTPS URLs
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sensor APIs V1");
        c.RoutePrefix = ""; // Swagger at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Apply CORS policy before authentication

app.UseAuthentication();  // Enable authentication
app.UseAuthorization();   // Enable authorization

app.MapControllers();  // Map controller endpoints

app.Run();
