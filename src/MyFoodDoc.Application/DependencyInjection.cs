using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Services;


namespace MyFoodDoc.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StatisticsOptions>(configuration.GetSection("Statistics"));
            services.Configure<EmailServiceOptions>(configuration.GetSection("EmailService"));
            services.Configure<StatisticsExportOptions>(configuration.GetSection("StatisticsExport"));

            return services;
        }
    }
}
