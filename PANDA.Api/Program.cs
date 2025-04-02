using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Panda.Api.Converters;
using PANDA.Api.Infrastructure;
using PANDA.Api.Services;
using PANDA.Api.Validation;
using PANDA.Api.Mapping;
using AutoMapper;
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
// JSON + FluentValidation
// ----------------------------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
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
// Optional: Validate mappings at startup
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// ----------------------------
// App Services
// ----------------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

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
app.MapControllers();
app.Run();
