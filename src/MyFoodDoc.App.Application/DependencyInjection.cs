using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Services;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MyFoodDoc.Application;
using MyFoodDoc.FatSecretClient;

namespace MyFoodDoc.App.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedFatSecretClient(configuration);
            services.AddSharedApplication();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IDiaryService, DiaryService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<ILexiconService, LexiconService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserHistoryService, UserHistoryService>();

            return services;
        }
    }       
}
