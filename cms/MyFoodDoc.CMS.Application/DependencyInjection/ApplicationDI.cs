using Microsoft.Extensions.DependencyInjection;
using MyFoodDoc.CMS.Application.Services;
using MyFoodDoc.CMS.Application.Services.Implementation;

namespace MyFoodDoc.CMS.Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
