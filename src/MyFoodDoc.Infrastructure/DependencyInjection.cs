using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Infrastructure.Persistence.Database;

namespace MyFoodDoc.Infrastructure
{
    public static class DependencyInjection
    {
        private const string ConnectionStringName = "DefaultConnection";

        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlServer(connectionString),
                ServiceLifetime.Transient
            );

            services.AddTransient<IApplicationContext, ApplicationContext>();

            return services;
        }
    }
}