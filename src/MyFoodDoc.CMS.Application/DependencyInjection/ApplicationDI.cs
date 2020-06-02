using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.FatSecretClient;

namespace MyFoodDoc.CMS.Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedFatSecretClient(configuration);

            return services;
        }
    }
}
