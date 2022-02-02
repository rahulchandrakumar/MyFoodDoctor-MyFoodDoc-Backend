using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using MyFoodDoc.App.Api.DependencyInjection;
using MyFoodDoc.App.Api.Middlewares;
using MyFoodDoc.App.Api.RouteConstraints;
using MyFoodDoc.App.Application;
using MyFoodDoc.App.Application.Clients;
using MyFoodDoc.App.Application.Clients.IdentityServer;
using MyFoodDoc.App.Application.Serialization;
using MyFoodDoc.App.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MyFoodDoc.App.Api
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddInfrastructure(Configuration, Environment);
            services.AddApplication(Configuration);

            //IdentityServer
            services.Configure<IdentityServerClientOptions>(Configuration.GetSection("IdentityServer"));

            var identityServerUrl = Configuration.GetValue<string>("IdentityServer:Address");

            services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
            {
                client.BaseAddress = new Uri(identityServerUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });


            /*
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new RsaSecurityKey(new RSACryptoServiceProvider(2048).ExportParameters(true)),
                        ValidAudience = "myfooddoc_api",
                        ValidIssuer = "http://localhost:50396",
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.FromMinutes(0)
                    };

                    /*
                    options.Authority = "http://localhost:50396";
                    options.Audience = "myfooddoc_api";
                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;
                    options.TokenValidationParameters.ValidateActor = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    options.TokenValidationParameters.ValidateIssuer = false;
                    
                });
             */

            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddDistributedMemoryCache();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityServerUrl;
                    options.ApiName = "myfooddoc_api";
                    options.RequireHttpsMetadata = false;
                    options.ApiSecret = "secret";

                    //options.EnableCaching = true;
                    //options.CacheDuration = TimeSpan.FromMinutes(10);// that's the default
                });

            /*
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                //option.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
            });
            */
            services.AddSwaggerDocumentation(Configuration);

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
                options.ConstraintMap.Add(DateRouteConstraint.Name, typeof(DateRouteConstraint));
            });

            services
                .AddControllers(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                         .RequireAuthenticatedUser()
                         .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Environment.IsProduction() ? Formatting.Indented : Formatting.None;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy(), false));
                    options.SerializerSettings.Converters.Add(new DateTimeConverter());
                    options.SerializerSettings.Converters.Add(new TimespanConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("MyFoodDoc.App.Application")));
                });

            // services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }

            app.UseApplicationExceptionHandler();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
