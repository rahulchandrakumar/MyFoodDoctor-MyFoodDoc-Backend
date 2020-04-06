using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.App.Api.DependencyInjection
{
    public static class SwaggerApplicationBuilderExtensions
    {
        // Obsolete Setting 'DescribeAllEnumsAsStrings' is required for System.Text.Json. 
        // See: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1269. 
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.IgnoreObsoleteProperties();
                options.IgnoreObsoleteActions();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Main API v1.0", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri(configuration.GetValue<string>("IdentityServer:Address") + "/connect/token"),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "myfooddoc_api", "MyFoodDoc.Api" }
                                }
                            }
                        }
                    });
                options.OperationFilter<AuthResponsesOperationFilter>();
                options.CustomSchemaIds(i => i.FullName);
                //options.AddFluentValidationRules();
                options.SchemaFilter<FluentValidationRules>();
                options.OperationFilter<FluentValidationOperationFilter>();
                options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
                options.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Format = "time" });
            });

            return services;
        }

        private class AuthResponsesOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                // Check for authorize attribute
                var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>();

                if (authAttributes.Any())
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }
        }
    }
}
