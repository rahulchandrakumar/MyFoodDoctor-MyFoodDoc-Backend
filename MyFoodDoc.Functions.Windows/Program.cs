using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.App.Application;
using MyFoodDoc.Application.Services;
using MyFoodDoc.Core.Configuration.ConfigurationMapper;
using MyFoodDoc.Infrastructure;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Windows
{
    public class Program
    {
        public static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.SetBasePath(Environment.CurrentDirectory)
                        .WithJsonMapping(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(Program).Namespace}.mapping.json"))
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                        .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
                })
                .ConfigureServices((hostingContext, services)  =>
                {   
                    services.AddOptions<EmailServiceOptions>()
                            .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("EmailService").Bind(settings));

                    var configuration = hostingContext.Configuration;

                    services.AddSharedInfrastructure(configuration, null, addTelemetry: false);
                    services.AddApplication(configuration);

                })
                .Build();

            await host.RunAsync();
        }
    }
}