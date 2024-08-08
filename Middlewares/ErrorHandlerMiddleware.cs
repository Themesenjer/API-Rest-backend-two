using System.Net;
using System.Text.Json;

namespace FacturasAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 si no es manejado
            string result;

            switch (exception)
            {
                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized; // 401
                    result = JsonSerializer.Serialize(new { code = (int)code, message = "No autorizado" });
                    break;
                case KeyNotFoundException _:
                    code = HttpStatusCode.NotFound; // 404
                    result = JsonSerializer.Serialize(new { code = (int)code, message = "No encontrado" });
                    break;
                default:
                    result = JsonSerializer.Serialize(new { code = (int)code, message = "Error en el servidor" });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
