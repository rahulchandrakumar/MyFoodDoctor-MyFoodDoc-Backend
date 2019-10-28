using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.App.Api.DependencyInjection;
using MyFoodDoc.App.Api.RouteConstraints;
using MyFoodDoc.App.Application.Helpers;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Api.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace MyFoodDoc.Application.Api
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
            services.AddOptions();

            services.Configure<IdentityServerClientOptions>(Configuration.GetSection("IdentityServer"));

            services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "http://localhost:8000";
                    options.Audience = "myfooddoc-app";
                    options.RequireHttpsMetadata = false;
                }, null);

            /*
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                //option.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
            });
            */
            services.AddSwaggerDocumentation();

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
                options.ConstraintMap.Add(DateRouteConstraint.Name, typeof(DateRouteConstraint));
            });

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Environment.IsProduction() ? Formatting.Indented : Formatting.None;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy(), false));
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<IApplicationContext>();
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
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

            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
