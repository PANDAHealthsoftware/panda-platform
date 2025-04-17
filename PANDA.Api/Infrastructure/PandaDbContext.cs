using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PANDA.Api.Models;
using PANDA.Domain.Entities;

namespace PANDA.Api.Infrastructure;

public class PandaDbContext : DbContext
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Clinician> Clinicians => Set<Clinician>();
    public DbSet<User> Users => Set<User>();

    private static readonly ValueConverter<DateTime, string> DateTimeToIsoConverter =
        new(v => v.ToString("o"), v => DateTime.Parse(v));

    private static readonly ValueConverter<DateOnly, DateTime> DateOnlyToDateTimeConverter =
        new(d => d.ToDateTime(TimeOnly.MinValue), d => DateOnly.FromDateTime(d));

    public PandaDbContext(DbContextOptions<PandaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PandaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
