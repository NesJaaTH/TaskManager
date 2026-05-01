using System.Diagnostics;

namespace TaskManager.WebApi.Middleware;

/// Log การเริ่มต้น สถานะเสร็จสิ้น และเวลาที่ใช้ของแต่ละ request
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// วัดเวลาที่ใช้ในการ execute request และ log วงจรชีวิตของ request
    public async Task InvokeAsync(HttpContext context)
    {
        // เริ่มวัดเวลาที่ใช้
        var stopwatch = Stopwatch.StartNew();
        
        // ใช้ trace identifier ที่สร้างให้อัตโนมัติสำหรับ correlation ข้าม logs
        var correlationId = context.TraceIdentifier;

        // Log การเริ่มต้น request
        _logger.LogInformation(
            "Request started: {Method} {Path} {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        try
        {
            // ส่งต่อไปยัง middleware ถัดไป
            await _next(context);
        }
        finally
        {
            // หยุดวัดเวลาและ log การเสร็จสิ้นไม่ว่าจะสำเร็จหรือล้มเหลว
            stopwatch.Stop();

            _logger.LogInformation(
                "Request completed: {Method} {Path} {StatusCode} {Duration}ms {CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }
    }
}

/// Extension method สำหรับเพิ่ม RequestLoggingMiddleware ใน pipeline
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}