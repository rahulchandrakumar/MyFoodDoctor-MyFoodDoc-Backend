using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.Core.Configuration.ConfigurationMapper;

namespace MyFoodDoc.App.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.WithJsonMapping("mapping.json");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
