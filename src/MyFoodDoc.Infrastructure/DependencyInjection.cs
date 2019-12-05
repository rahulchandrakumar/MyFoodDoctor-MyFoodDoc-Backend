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
            {
                //options.UseInMemoryDatabase(databaseName: "Add_writes_to_database");
                //options.UseSqlServer("Server=localhost;Database=MyFoodDoc2;Trusted_Connection=True;");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationContext, ApplicationContext>();

            return services;
        }
    }
}
