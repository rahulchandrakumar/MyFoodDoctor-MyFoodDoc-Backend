using System;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.Application;
using MyFoodDoc.Infrastructure;
using MyFoodDoc.Core.Configuration.ConfigurationMapper;
using MyFoodDoc.FatSecretClient;
using MyFoodDoc.AppStoreClient;
using MyFoodDoc.GooglePlayStoreClient;
using MyFoodDoc.FirebaseClient;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MyFoodDoc.Functions.Startup))]
namespace MyFoodDoc.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .WithJsonMapping(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{this.GetType().Namespace}.mapping.json"))
                .Build();

            builder.Services.AddSingleton<IConfiguration>(configuration);

            builder.Services.AddSharedInfrastructure(configuration, null);
            builder.Services.AddSharedApplication(configuration);
            builder.Services.AddSharedAppStoreClient(configuration);
            builder.Services.AddSharedGooglePlayStoreClient(configuration);
            builder.Services.AddSharedFatSecretClient(configuration);
            builder.Services.AddSharedFirebaseClient(configuration);
        }
    }
}
