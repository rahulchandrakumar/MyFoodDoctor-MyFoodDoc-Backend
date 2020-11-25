using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.FirebaseClient.Abstractions;
using MyFoodDoc.FirebaseClient.Clients;

namespace MyFoodDoc.FirebaseClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedFirebaseClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebaseClientOptions>(
                configuration.GetSection("Firebase"));

            services.AddScoped<IFirebaseClient, Clients.FirebaseClient>();

            return services;
        }
    }
}
