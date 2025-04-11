using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Api.Mapping;
using PANDA.Api.Services;
using PANDA.Api.Services.Appointment;
using PANDA.Api.Services.Clinician;
using PANDA.Api.Services.Patient;
using PANDA.Api.Validation;
using PANDA.Shared.Converters;
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
// JSON + FluentValidation
// ----------------------------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PatientDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AppointmentDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAppointmentDtoValidator>();

// ----------------------------
// Swagger & Localization
// ----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization();

// ----------------------------
// Entity Framework Core (SQLite)
// ----------------------------
builder.Services.AddDbContext<PandaDbContext>(options =>
    options.UseSqlite("Data Source=panda.db"));

// ----------------------------
// AutoMapper (Manual Setup)
// ----------------------------
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// ----------------------------
// App Services
// ----------------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IClinicianService, ClinicianService>();

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

// ✅ CORS must go **before** controllers
app.UseCors("AllowPANDAview");

app.MapControllers();
app.Run();
