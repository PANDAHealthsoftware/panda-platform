using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using PANDA.Api.Extensions;
using PANDA.Api.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

// ---------------------------------------------------------
// Serilog: Structured logging with console + file output
// ---------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region Azure Key Vault

// ---------------------------------------------------------
// Load secrets securely from Azure Key Vault
// (e.g., DB connection string, other sensitive config)
// ---------------------------------------------------------
builder.Configuration.AddAzureKeyVault(
    new Uri("https://panda-keyvault-uk.vault.azure.net/"),
    new DefaultAzureCredential());

#endregion

#region CORS & Localization

// ---------------------------------------------------------
// Allow local frontend (e.g., React/Blazor SPA) access
// ---------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPANDAview", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Stub for future localization support
builder.Services.AddLocalization();

#endregion

#region Database

// ---------------------------------------------------------
// Read connection string (e.g. from Key Vault)
// ---------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string not configured.");
}

builder.Services.AddDbContext<PandaDbContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

#region Authentication & Authorization

// ---------------------------------------------------------
// Microsoft Identity Web integration for JWTs from Azure AD
// Token audience and tenant config is read from appsettings.json
// ---------------------------------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Role/claim-based policy support (can be extended later)
builder.Services.AddAuthorization();

#endregion

#region Custom Services & Swagger

// ---------------------------------------------------------
// Register DI services and enable Swagger with JWT support
// ---------------------------------------------------------
builder.Services
    .AddApplicationServices()
    .AddSwaggerWithSecurity(); // custom extension to wire Swagger auth header

#endregion

#region Build App + DB Seeding

var app = builder.Build();

// ---------------------------------------------------------
// Ensure DB is seeded with any required initial data
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PandaDbContext>();
    await DbInitializer.SeedAsync(db); // assumes async seed logic
}

#endregion

#region HTTP Request Pipeline

// ---------------------------------------------------------
// Dev tools: Swagger UI
// ---------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowPANDAview"); // CORS must come before auth

// ---------------------------------------------------------
// Authentication → Authorization → Controllers
// ---------------------------------------------------------
app.UseAuthentication();   // Validate bearer token
app.UseAuthorization();    // Check claims/roles if required

app.MapControllers();      // API endpoints

app.Run();

#endregion
