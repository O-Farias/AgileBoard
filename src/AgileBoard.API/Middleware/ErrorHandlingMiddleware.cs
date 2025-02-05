using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AgileBoard.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                StatusCode = GetStatusCode(exception),
                Message = GetMessage(exception),
                TraceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }

        private static string GetMessage(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => "Recurso não encontrado.",
                ArgumentException => "Requisição inválida.",
                UnauthorizedAccessException => "Acesso não autorizado.",
                _ => "Ocorreu um erro interno no servidor."
            };
        }
    }
}