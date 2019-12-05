using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.Application.Abstractions;
using System.Linq;

namespace MyFoodDoc.Infrastructure
{
    public static class Configuration
    {
        public static void UseSharedInfrastructure(this IApplicationBuilder app)
        {
            var seeders = app.ApplicationServices.GetServices<ISeed>();
            if (seeders?.Count() > 0)
            {
                var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var context = (IApplicationContext)scope.ServiceProvider.GetService(typeof(IApplicationContext));
                    foreach (var seeder in seeders)
                        seeder.SeedData(context);
                }
            }
        }
    }
}
