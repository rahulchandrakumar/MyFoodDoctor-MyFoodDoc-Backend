using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.Application.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MyFoodDoc.Auth.Api
{
    public class Startup
    {
        private readonly string ApiOrigin = "api-origin";

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName: "Users"));
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            */

            var builder = services.AddIdentityServer(options =>
            {
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
            })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());
                //.AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }

            services.AddSwaggerDocumentation();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(ApiOrigin, builder =>
                {
                    builder.WithOrigins("https://myfooddoc-mock-api.azurewebsites.net");
                });
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }
            app.UseStaticFiles();
            app.UseCors(ApiOrigin);
            app.UseIdentityServer();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
