using Microsoft.AspNetCore.Diagnostics;

namespace Estoque.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;
                        var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                        logger.LogError(ex, "Exceção não tratada capturada pelo middleware global");
                        await context.Response.WriteAsync($"{{\"erro\":\"{ex.Message}\"}}");
                    }
                });
            });
        }
    }
}
