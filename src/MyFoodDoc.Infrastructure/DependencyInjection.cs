using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Services;
using MyFoodDoc.Infrastructure.Persistence.Database;
using SendGrid;

namespace MyFoodDoc.Infrastructure
{
    public static class DependencyInjection
    {
        private const string ConnectionStringName = "DefaultConnection";

        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, bool addTelemetry = true)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            services.AddDbContext<ApplicationContext>(options =>
                    options.UseSqlServer(connectionString),
                ServiceLifetime.Transient
            );

            services.AddTransient<IApplicationContext>(provider => provider.GetService<ApplicationContext>());

            services.AddScoped<ISendGridClient>(client =>
                new SendGridClient(configuration.GetSection("EmailService").Get<EmailServiceOptions>().SendGridApiKey));

            services.AddScoped<IEmailService, EmailService>();

            if (addTelemetry)
            {   // The following line enables Application Insights telemetry collection.
                services.AddApplicationInsightsTelemetry();
            }

            return services;
        }
    }
}