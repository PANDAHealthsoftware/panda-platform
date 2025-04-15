using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PANDA.Api.Infrastructure;
using PANDA.Api.Mapping;
using PANDA.Api.Services.Appointment;
using PANDA.Api.Services.Clinician;
using PANDA.Api.Services.Patient;
using PANDA.Api.Validation;
using PANDA.Shared.Converters;
using PANDA.Shared.DTOs.Patient;
using PANDA.Shared.Security;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// Logging
// ----------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ----------------------------
// CORS
// ----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPANDAview", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----------------------------
// Controllers + FluentValidation
// ----------------------------
builder.Services
    .AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<UpdatePatientDtoValidator>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
    });

// ----------------------------
// Swagger & Localization
// ----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PANDA API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddLocalization();

// ----------------------------
// Entity Framework Core (SQLite)
// ----------------------------
builder.Services.AddDbContext<PandaDbContext>(options =>
    options.UseSqlite("Data Source=panda.db"));

// ----------------------------
// AutoMapper
// ----------------------------
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// ----------------------------
// Application Services
// ----------------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IClinicianService, ClinicianService>();
builder.Services.AddTransient<IValidator<UpdatePatientDto>, UpdatePatientDtoValidator>();

// ----------------------------
// JWT Authentication & Authorization
// ----------------------------
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super-secure-signing-key-must-be-at-least-32-chars";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "https://panda-api.local";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = "panda-api",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole(Roles.Admin));
    options.AddPolicy("RequireClinician", policy => policy.RequireRole(Roles.Clinician));
    options.AddPolicy("RequireReception", policy => policy.RequireRole(Roles.Reception));
});
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
// ----------------------------
// Build App
// ----------------------------
var app = builder.Build();

// ----------------------------
// Middleware
// ----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowPANDAview");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Console.WriteLine("🧩 Tokens assembly version: " +
                  typeof(Microsoft.IdentityModel.Tokens.TokenValidationParameters).Assembly.FullName);

Console.WriteLine("🔐 JWT handler assembly version: " +
                  typeof(System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler).Assembly.FullName);
app.Run();
