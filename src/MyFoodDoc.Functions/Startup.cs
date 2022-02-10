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
using MyFoodDoc.Functions.Abstractions;
using MyFoodDoc.Functions.Services;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Application.Services;

[assembly: FunctionsStartup(typeof(MyFoodDoc.Functions.Startup))]
namespace MyFoodDoc.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .WithJsonMapping(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{this.GetType().Namespace}.mapping.json"));

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<StatisticsOptions>()
                    .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Statistics").Bind(settings));

            builder.Services.AddOptions<EmailServiceOptions>()
                    .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("EmailService").Bind(settings));

            builder.Services.AddOptions<StatisticsExportOptions>()
                    .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("StatisticsExport").Bind(settings));

            var configuration = builder.GetContext().Configuration;

            builder.Services.AddTransient<IUserStatsService, UserStatsService>();

            builder.Services.AddSharedInfrastructure(configuration, null, addTelemetry: false);
            
            builder.Services.AddSharedAppStoreClient(configuration);
            builder.Services.AddSharedGooglePlayStoreClient(configuration);
            builder.Services.AddSharedFatSecretClient(configuration);
            builder.Services.AddSharedFirebaseClient(configuration);
        }
    }
}
