using Microsoft.Extensions.DependencyInjection;

namespace MyFoodDoc.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
