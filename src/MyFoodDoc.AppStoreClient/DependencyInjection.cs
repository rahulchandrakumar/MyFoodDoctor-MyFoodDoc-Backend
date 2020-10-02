using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.AppStoreClient.Abstractions;
using MyFoodDoc.AppStoreClient.Clients;

namespace MyFoodDoc.AppStoreClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedAppStoreClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppStoreClientOptions>(configuration.GetSection("AppStore"));

            var verifyReceiptUrl = configuration.GetValue<string>("AppStore:VerifyReceiptUrl");

            services.AddHttpClient<IAppStoreClient, Clients.AppStoreClient>(client =>
            {
                client.BaseAddress = new Uri(verifyReceiptUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
