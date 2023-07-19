using Project.Core.Exeptions;
using Project.Core.Models;
using System.Text.Json;

namespace Project.Api.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary/>
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    AlreadyExistException => 400,
                    WrongPasswordException => 400,
                    InvalidTokenException => 400,
                    NotFoundException => 404,
                    NotAuthorizedException => 401,
                    _ => 500,
                };
                var respone = new ExceptionResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                var result = JsonSerializer.Serialize(respone, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
