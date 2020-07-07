using System;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.Infrastructure;
using MyFoodDoc.Core.Configuration.ConfigurationMapper;
using MyFoodDoc.FatSecretClient;

[assembly: WebJobsStartup(typeof(MyFoodDoc.FatSecretSynchronization.Startup))]

namespace MyFoodDoc.FatSecretSynchronization
{
    class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
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

            builder.Services.AddSharedFatSecretClient(configuration);
        }
    }
}
