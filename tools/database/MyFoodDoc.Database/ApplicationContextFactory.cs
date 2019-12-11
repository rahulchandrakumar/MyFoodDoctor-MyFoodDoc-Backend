using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyFoodDoc.Infrastructure.Persistence.Database;

namespace MyFoodDoc.Database
{
    public class ApplicationContextFactory : AbstractDesignTimeDbContextFactory<ApplicationContext>
    {
        protected override ApplicationContext CreateNewInstance(DbContextOptions<ApplicationContext> options)
        {
            var context = new ApplicationContext(options);

            context.WithSeeding = true;

            return context;
        }
    }
}
