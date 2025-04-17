using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PANDA.Domain.Entities;

namespace PANDA.Api.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.Property(p => p.DateOfBirth)
            .HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));

        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName");
            name.Property(n => n.LastName).HasColumnName("LastName");
        });
    }
}