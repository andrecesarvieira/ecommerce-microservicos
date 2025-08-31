using Microsoft.AspNetCore.Diagnostics;

namespace Vendas.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
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
                        await context.Response.WriteAsync($"{{\"erro\":\"{ex.Message}\"}}");
                    }
                });
            });
        }
    }
}
