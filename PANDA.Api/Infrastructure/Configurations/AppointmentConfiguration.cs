using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PANDA.Api.Models;

namespace PANDA.Api.Infrastructure.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);

        builder.Property(a => a.AppointmentDate)
            .HasConversion(v => v.ToString("o"), v => DateTime.Parse(v));

        builder.Property(a => a.Status)
            .HasConversion<int>();
        
        builder.Property(a => a.Reason)
            .HasMaxLength(250);
    }
}