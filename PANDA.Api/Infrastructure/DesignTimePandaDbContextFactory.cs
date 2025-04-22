using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PANDA.Api.Infrastructure;

public class DesignTimePandaDbContextFactory : IDesignTimeDbContextFactory<PandaDbContext>
{
    public PandaDbContext CreateDbContext(string[] args)
    {
        // 1. Load basic config so we can get vault URI
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // 2. Manually connect to Azure Key Vault
        var keyVaultUri = new Uri("https://panda-keyvault-uk.vault.azure.net/");
        var client = new SecretClient(keyVaultUri, new DefaultAzureCredential());

        // 3. Fetch connection string from Key Vault
        var secret = client.GetSecret("ConnectionStrings--DefaultConnection").Value;
        var connectionString = secret.Value;

        // 4. Configure EF DbContext
        var optionsBuilder = new DbContextOptionsBuilder<PandaDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new PandaDbContext(optionsBuilder.Options);
    }
}