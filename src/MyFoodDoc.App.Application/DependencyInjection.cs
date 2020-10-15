using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Services;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MyFoodDoc.Application;
using MyFoodDoc.FatSecretClient;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Services;
using MyFoodDoc.AppStoreClient;

namespace MyFoodDoc.App.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedAppStoreClient(configuration);
            services.AddSharedFatSecretClient(configuration);
            services.AddSharedApplication(configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IDiaryService, DiaryService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<ILexiconService, LexiconService>();
            services.AddScoped<ITargetService, TargetService>();
            services.AddScoped<IMethodService, MethodService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserHistoryService, UserHistoryService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }       
}
