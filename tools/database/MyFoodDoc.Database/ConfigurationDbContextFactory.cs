using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace MyFoodDoc.Database
{
    public class ConfigurationDbContextFactory : AbstractDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options)
        {
            var context = new ConfigurationDbContext(options, new ConfigurationStoreOptions());

            return context;
        }
    }
}
