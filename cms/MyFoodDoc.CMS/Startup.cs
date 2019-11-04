using DotNetify;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyFoodDoc.CMS
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            #region CORS
            services.AddCors(o => o.AddPolicy("AllPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            #endregion

            #region dotnetify
            services.AddMemoryCache();
            services.AddSignalR();
            services.AddDotNetify();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region CORS
            app.UseCors("AllPolicy");
            #endregion

            app.UseRouting();

            #region dotnetify
            app.UseWebSockets();
#pragma warning disable CS0618 // Type or member is obsolete
            app.UseSignalR(routes => routes.MapDotNetifyHub());
#pragma warning restore CS0618 // Type or member is obsolete
            app.UseDotNetify();
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
