using Microsoft.EntityFrameworkCore;
using PANDA.Api.Extensions;
using PANDA.Api.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPANDAview", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddLocalization();

builder.Services.AddDbContext<PandaDbContext>(options =>
    options.UseSqlite("Data Source=panda.db"));

builder.Services
    .AddApplicationServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerWithSecurity();

var app = builder.Build();

// Seed DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PandaDbContext>();
    await DbInitializer.SeedAsync(db);
}

// Middleware
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
app.Run();