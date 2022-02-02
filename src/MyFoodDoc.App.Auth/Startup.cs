using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.App.Infrastructure;
using MyFoodDoc.Application.Entities;
using System.Linq;

namespace MyFoodDoc.App.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Config.GetApiScopes())
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();

                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration, Environment);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                var identityServerIssuerUri = Configuration.GetValue<string>("IdentityServer:IssuerUri");

                options.IssuerUri = identityServerIssuerUri;

                options.Authentication.CookieAuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                /*
                options.Endpoints.EnableAuthorizeEndpoint = false;
                options.Endpoints.EnableCheckSessionEndpoint = false;
                options.Endpoints.EnableDeviceAuthorizationEndpoint = false;
                options.Endpoints.EnableUserInfoEndpoint = false;
                options.Endpoints.EnableDiscoveryEndpoint = true;
                options.Endpoints.EnableEndSessionEndpoint = false;
                options.Endpoints.EnableIntrospectionEndpoint = false;
                options.Endpoints.EnableJwtRequestUri = true;
                options.Endpoints.EnableTokenEndpoint = true;
                options.Endpoints.EnableTokenRevocationEndpoint = false;
                options.Endpoints.EnableUserInfoEndpoint = false;
                */
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString);
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString);
            })
            //.AddInMemoryIdentityResources(Config.GetIdentityResources())
            //.AddInMemoryApiScopes(Config.GetApiScopes())
            //.AddInMemoryApiResources(Config.GetApiResources())
            //.AddInMemoryClients(Config.GetClients())
            .AddAspNetIdentity<User>();

            //TODO: Use builder.AddSigningCredential for Staging and Production
            if (Environment.IsDevelopment() || Environment.IsStaging() || Environment.IsProduction())
            {
                builder.AddDeveloperSigningCredential();
            }

            /*
            services.AddCors(options =>
            {
                options.AddPolicy(ApiOrigin, builder =>
                {
                    builder.WithOrigins("https://myfooddoc-mock-api.azurewebsites.net");
                });
            });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            /*
            var identityServerIssuerUri = Configuration.GetValue<string>("IdentityServer:IssuerUri");
            
            app.Use(async (ctx, next) =>
            {
                ctx.SetIdentityServerOrigin(identityServerIssuerUri);
                await next();
            });
            */

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseStaticFiles();
            //app.UseCors(ApiOrigin);

            var identityServerIssuerUri = Configuration.GetValue<string>("IdentityServer:IssuerUri");

            app.Use(async (ctx, next) =>
            {
                ctx.SetIdentityServerOrigin(identityServerIssuerUri);
                await next();
            });

            app.UseIdentityServer();

            //app.UseRouting();
        }
    }
}
