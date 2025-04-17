using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PANDA.Api.Models;

namespace PANDA.Api.Infrastructure.Configurations;

public class ClinicianConfiguration : IEntityTypeConfiguration<Clinician>
{
    public void Configure(EntityTypeBuilder<Clinician> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}