using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace MyFoodDoc.Database
{
    public class PersistedGrantDbContextFactory : AbstractDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options)
        {
            var context = new PersistedGrantDbContext(options, new OperationalStoreOptions());

            return context;
        }
    }
}
