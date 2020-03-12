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

            services.AddHttpClient<IFatSecretClient, Clients.FatSecretClient>(client =>
            {
                client.BaseAddress = new Uri(fatSecretUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
