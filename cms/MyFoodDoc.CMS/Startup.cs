using DotNetify;
using DotNetify.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyFoodDoc.CMS.Application.DependencyInjection;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Auth;
using MyFoodDoc.CMS.Auth.Implementation;
using MyFoodDoc.CMS.Infrastructure.Dependencyinjection;
using MyFoodDoc.CMS.Infrastructure.Persistence;
using MyFoodDoc.Infrastructure;
using System;
using System.Text;

namespace MyFoodDoc.CMS
{
    public class Startup
    {
        private TokenValidationParameters _tokenValidationParameters;

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region ASP
            services.AddMvc(o => o.EnableEndpointRouting = false);
            #endregion

            #region DI
            services.AddSharedInfrastructure(Configuration, Environment);

            services.AddTransient<ICustomAuthenticationService, DebugAuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<ILexiconService, LexiconService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IWebViewService, WebViewService>();
            services.AddApplicationDI();
            services.AddAzureStorage(Configuration.GetConnectionString("BlobStorageConnectionString"), Configuration.GetValue<Uri>("CDN"));
            #endregion

            #region CORS
            services.AddCors();
            #endregion

            #region dotnetify
            services.AddMemoryCache();
            services.AddSignalR().AddMessagePackProtocol();
            services.AddDotNetify();
            #endregion

            #region Auth
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(Configuration["AuthSecret"]);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = _tokenValidationParameters;
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region CORS
            app.UseCors(builder =>
                builder.WithOrigins(Configuration["Cors"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );
            #endregion

            #region ASP
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            #endregion

            #region dotnetify
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DotNetifyHub>("/dotnetify");
            });
            app.UseDotNetify(config => {
                config.UseFilter<AuthorizeFilter>();
                config.UseJwtBearerAuthentication(_tokenValidationParameters);
            });
            #endregion

            #region SPA-debug
            if (env.EnvironmentName == "VisualStudio")
            {
                app.UseSpa(spa =>
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8081");
                });
            }
            #endregion
        }
    }
}
