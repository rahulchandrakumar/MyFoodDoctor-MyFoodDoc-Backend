using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Middlewares
{
    public class ApplicationExceptionHandlerMiddleware
    {
        private const string JsonContentType = "application/json";

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;


        public ApplicationExceptionHandlerMiddleware(RequestDelegate next, ILogger<ApplicationExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<ApplicationExceptionHandlerMiddleware>));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Failures);
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    break;
                case ConflictException conflictException:
                    code = HttpStatusCode.Conflict;
                    result = conflictException.Message;
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case DeleteFailureException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            context.Response.ContentType = JsonContentType;
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonConvert.SerializeObject(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class ApplicationExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationExceptionHandler(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<ApplicationExceptionHandlerMiddleware>();
        }
    }
}
