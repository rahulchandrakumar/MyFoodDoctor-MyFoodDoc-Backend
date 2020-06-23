using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MyFoodDoc.App.Api.DependencyInjection
{
    public static class SwaggerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.List);
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
            });
            return app;
        }
    }
}
