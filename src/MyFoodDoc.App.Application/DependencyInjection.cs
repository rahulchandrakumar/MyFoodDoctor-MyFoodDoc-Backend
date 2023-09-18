using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.AokClient;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Services;
using MyFoodDoc.Application;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Services;
using MyFoodDoc.AppStoreClient;
using MyFoodDoc.FatSecretClient;
using MyFoodDoc.GooglePlayStoreClient;
using SendGrid;
using System.Reflection;
using MyFoodDoc.App.Application.Abstractions.V2;
using MyFoodDoc.App.Application.Services.V2;

namespace MyFoodDoc.App.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedAppStoreClient(configuration);
            services.AddSharedGooglePlayStoreClient(configuration);
            services.AddSharedFatSecretClient(configuration);
            services.AddSharedApplication(configuration);
            services.AddSharedAokClient(configuration);

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
            services.AddScoped<IUserServiceV2, UserServiceV2>();
            services.AddScoped<IDiaryServiceV2, DiaryServiceV2>();
            services.AddScoped<ITargetServiceV2, TargetServiceV2>();
            services.AddScoped<IFoodServiceV2, FoodServiceV2>();

            services.AddScoped<ISendGridClient>(client =>
                new SendGridClient(configuration.GetSection("EmailService").Get<EmailServiceOptions>().SendGridApiKey));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPsychogrammService, PsychogrammService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IHtmlPdfService, HtmlPdfService>();

            return services;
        }
    }
}
