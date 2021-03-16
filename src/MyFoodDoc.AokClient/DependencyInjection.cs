using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.AokClient.Abstractions;
using MyFoodDoc.AokClient.Clients;
using System;

namespace MyFoodDoc.AokClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedAokClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AokClientOptions>(configuration.GetSection("Aok"));

            var aokServiceUrl = configuration.GetValue<string>("AokClient:Address");

            services.AddHttpClient<IAokClient, Clients.AokClient>(client =>
            {
                client.BaseAddress = new Uri(aokServiceUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
