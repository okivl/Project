using Project.Api.Middlewares;

namespace Project.Api.Extensions
{
    /// <summary/>
    public static class MiddlewareExtension
    {
        /// <summary/>
        public static void UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }

    }
}
