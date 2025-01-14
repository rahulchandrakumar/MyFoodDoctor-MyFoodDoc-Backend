﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFoodDoc.App.Infrastructure.Azure.Queue.Abstractions;
using MyFoodDoc.App.Infrastructure.Azure.Queue;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Infrastructure;
using MyFoodDoc.Infrastructure.Persistence.Database;

namespace MyFoodDoc.App.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddLogging();

            services.AddSharedInfrastructure(configuration, environment);

            services.AddIdentity<User, IdentityRole<string>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.Tokens.PasswordResetTokenProvider = ResetPasswordTokenProvider.ProviderKey;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<ResetPasswordTokenProvider>(ResetPasswordTokenProvider.ProviderKey);

            services.AddScoped<IQueueService, QueueService>();

            return services;
        }
    }
}
