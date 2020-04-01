using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.FatSecretClient.Abstractions;
using MyFoodDoc.FatSecretClient.Clients;

namespace MyFoodDoc.FatSecretClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedFatSecretClient(this IServiceCollection services, IConfiguration configuration)
        {
            //FatSecretIdentityServer
            services.Configure<FatSecretIdentityServerClientOptions>(configuration.GetSection("FatSecretIdentityServer"));

            var fatSecretIdentityServerUrl = configuration.GetValue<string>("FatSecretIdentityServer:Address");

            services.AddHttpClient<IFatSecretIdentityServerClient, FatSecretIdentityServerClient>(client =>
            {
                client.BaseAddress = new Uri(fatSecretIdentityServerUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //FatSecret
            services.Configure<FatSecretClientOptions>(configuration.GetSection("FatSecret"));

            var fatSecretUrl = configuration.GetValue<string>("FatSecret:Address");
            /*
            TODO: Check relevance. Add switch between O1 and O2 if needed
            services.AddHttpClient<IFatSecretClient, FatSecretClientO2>(client =>
            {
                client.BaseAddress = new Uri(fatSecretUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            */
            services.AddHttpClient<IFatSecretClient, FatSecretClientO1>(client =>
            {
                client.BaseAddress = new Uri(fatSecretUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
