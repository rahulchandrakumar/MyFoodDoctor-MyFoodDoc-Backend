using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.GooglePlayStoreClient.Abstractions;
using MyFoodDoc.GooglePlayStoreClient.Clients;

namespace MyFoodDoc.GooglePlayStoreClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedGooglePlayStoreClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GooglePlayStoreClientOptions>(configuration.GetSection("GooglePlayStore"));

            services.AddScoped<IGooglePlayStoreClient, Clients.GooglePlayStoreClient>();

            return services;
        }
    }
}
