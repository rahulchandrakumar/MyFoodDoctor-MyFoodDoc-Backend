using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.Application;
using MyFoodDoc.AppStoreClient;
using MyFoodDoc.Core.Configuration.ConfigurationMapper;
using MyFoodDoc.FatSecretClient;
using MyFoodDoc.FirebaseClient;
using MyFoodDoc.Functions.Abstractions;
using MyFoodDoc.Functions.Services;
using MyFoodDoc.GooglePlayStoreClient;
using MyFoodDoc.Infrastructure;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions;

public class Program
{

    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration(config => config
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .AddEnvironmentVariables()
                .WithJsonMapping(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(Program).Namespace}.mapping.json"))
            )
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddSharedApplication(context.Configuration);
                services.AddSharedInfrastructure(context.Configuration, null);
                services.AddSharedAppStoreClient(context.Configuration);
                services.AddSharedGooglePlayStoreClient(context.Configuration);
                services.AddSharedFatSecretClient(context.Configuration);
                services.AddSharedFirebaseClient(context.Configuration);
                services.AddTransient<IUserStatsService, UserStatsService>();

            })
            .Build();

        await host.RunAsync();
    }
}
