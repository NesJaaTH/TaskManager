using System.Net;
using System.Text.Json;

namespace TaskManager.WebApi.Middleware;


/// จับ exception ที่เกิดขึ้นระหว่าง request pipeline และส่งกลับ JSON error response ที่มีโครงสร้างชัดเจน
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// เรียก middleware ถัดไปใน pipeline พร้อมจับ exception ที่เกิดขึ้น
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // ส่งต่อไปยัง middleware ถัดไปใน pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log รายละเอียดของ exception สำหรับ debugging
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            
            // จัดการ exception และส่ง error response กลับ
            await HandleExceptionAsync(context, ex);
        }
    }

    /// แปลงประเภท exception เป็น HTTP status code ที่เหมาะสม และส่ง JSON response กลับ
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        // แปลงประเภท exception เป็น HTTP status code ที่เหมาะสม
        var (statusCode, message) = exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            InvalidOperationException => (HttpStatusCode.Conflict, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An internal error occurred")
        };

        response.StatusCode = (int)statusCode;

        // ส่ง JSON error response ที่มีโครงสร้าง
        var result = JsonSerializer.Serialize(new
        {
            error = message,
            statusCode = (int)statusCode
        });

        await response.WriteAsync(result);
    }
}

/// Extension method สำหรับเพิ่ม ExceptionHandlingMiddleware ใน pipeline
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}