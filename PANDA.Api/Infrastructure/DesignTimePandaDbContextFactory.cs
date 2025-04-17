using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PANDA.Api.Infrastructure;

namespace PANDA.Api.Infrastructure;

public class DesignTimePandaDbContextFactory : IDesignTimeDbContextFactory<PandaDbContext>
{
    public PandaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PandaDbContext>();
        optionsBuilder.UseSqlite("Data Source=panda.db"); // Same connection as runtime

        return new PandaDbContext(optionsBuilder.Options);
    }
}