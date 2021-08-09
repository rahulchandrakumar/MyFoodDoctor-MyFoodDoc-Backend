using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyFoodDoc.CMS.Application.Common;
using MyFoodDoc.CMS.Application.DependencyInjection;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Auth;
using MyFoodDoc.CMS.Auth.Implementation;
using MyFoodDoc.CMS.Hubs;
using MyFoodDoc.CMS.Infrastructure;
using MyFoodDoc.CMS.Infrastructure.Common;
using MyFoodDoc.CMS.Infrastructure.Persistence;
using MyFoodDoc.Infrastructure;
using System;
using System.Text;
using System.Threading.Tasks;

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
            #region SPA
            services.AddSpaStaticFiles(configuration =>
            {
                // In production, the Vue.js files will be served from this directory
                configuration.RootPath = "app/dist";
            });
            #endregion

            #region ASP
            services.AddResponseCompression();
            services.AddMvc(o => o.EnableEndpointRouting = false);
            #endregion

            #region DI
            services.AddSharedInfrastructure(Configuration, Environment);

            services.AddSingleton<IHashingManager, HashingManager>();
            services.AddTransient<ICustomAuthenticationService, CustomAuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<ILexiconService, LexiconService>();
            services.AddTransient<ILexiconCategoryService, LexiconCategoryService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IWebViewService, WebViewService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IInsuranceService, InsuranceService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IIngredientService, IngredientService>();
            services.AddTransient<IOptimizationAreaService, OptimizationAreaService>();
            services.AddTransient<ITargetService, TargetService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IChapterService, ChapterService>();
            services.AddTransient<ISubchapterService, SubchapterService>();
            services.AddTransient<IMethodService, MethodService>();
            services.AddTransient<IMethodMultipleChoiceService, MethodMultipleChoiceService>();
            services.AddTransient<IMethodTextService, MethodTextService>();
            services.AddTransient<IDietService, DietService>();
            services.AddTransient<IIndicationService, IndicationService>();
            services.AddTransient<IMotivationService, MotivationService>();
            services.AddTransient<IScaleService, ScaleService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IChoiceService, ChoiceService>();
            services.AddApplicationDI(Configuration);
            services.AddAzureStorage(Configuration.GetConnectionString("BlobStorageConnection"), Configuration.GetValue<Uri>("CDN"));
            services.AddSeeds(Environment);
            #endregion

            #region CORS
            services.AddCors();
            #endregion

            #region SignalR
            services.AddMemoryCache();
            services.AddSignalR().AddMessagePackProtocol();
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
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/edit-states")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region DI
            app.UseSharedInfrastructure();
            #endregion

            #region CORS
            var cors = Configuration["Cors"];
            if (!string.IsNullOrWhiteSpace(cors))
            {
                app.UseCors(builder =>
                    builder.WithOrigins(cors)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );
            }
            #endregion

            #region ASP
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            #endregion

            #region SignalR
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<EditStateHub>("/edit-states");
            });
            #endregion

            #region SPA
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "app";

                if (env.EnvironmentName == "VisualStudio")
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }
            });
            #endregion
        }
    }
}
