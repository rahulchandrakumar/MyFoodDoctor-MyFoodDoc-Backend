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
                var context = app.ApplicationServices.GetRequiredService<IApplicationContext>();
                foreach (var seeder in seeders)
                    seeder.SeedData(context);
            }
        }
    }
}
