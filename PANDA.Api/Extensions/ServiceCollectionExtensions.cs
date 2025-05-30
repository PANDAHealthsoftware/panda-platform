using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PANDA.Api.Mapping;
using PANDA.Api.Services.Appointment;
using PANDA.Api.Services.Auth;
using PANDA.Api.Services.Clinician;
using PANDA.Api.Services.Patient;
using PANDA.Api.Services.User;
using PANDA.Api.Validation;
using PANDA.Shared.DTOs.Patient;
using PANDA.Shared.Security;

namespace PANDA.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // FluentValidation
        services.AddControllers()
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<UpdatePatientDtoValidator>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new PANDA.Shared.Converters.DateOnlyJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new PANDA.Shared.Converters.DateTimeOffsetJsonConverter());
            });

        // App Services
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IClinicianService, ClinicianService>();
        services.AddTransient<IValidator<UpdatePatientDto>, UpdatePatientDtoValidator>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserService, UserService>();
        

        // AutoMapper
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        services.AddSingleton(mapperConfig.CreateMapper());
        mapperConfig.AssertConfigurationIsValid();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtKey = config["JwtSettings:Key"];
        var jwtIssuer = config["JwtSettings:Issuer"];
        var jwtAudience = config["JwtSettings:Audience"];
        Console.WriteLine($"JWT Setup: Issuer={jwtIssuer}, Audience={jwtAudience}, Key={jwtKey?.Substring(0,8)}...");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdmin", policy => policy.RequireRole(Roles.Admin));
            options.AddPolicy("RequireClinician", policy => policy.RequireRole(Roles.Clinician));
            options.AddPolicy("RequireReception", policy => policy.RequireRole(Roles.Reception));
        });

        return services;
    }

    public static IServiceCollection AddSwaggerWithSecurity(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
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
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
