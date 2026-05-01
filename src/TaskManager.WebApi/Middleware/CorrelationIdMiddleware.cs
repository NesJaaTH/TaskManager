namespace TaskManager.WebApi.Middleware;

/// <summary>
/// เพิ่มหรือดึง correlation ID สำหรับ distributed tracing ข้าม services
/// อ่านจาก X-Correlation-ID header ถ้ามี ถ้าไม่มีจะสร้างใหม่
/// </summary>
public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    public const string CorrelationIdItem = "CorrelationId";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// ประมวลผล request และทำให้มั่นใจว่ามี correlation ID พร้อมใช้งานตลอด pipeline
    /// </summary>
    public Task InvokeAsync(HttpContext context)
    {
        // ดึงหรือสร้าง correlation ID สำหรับ request นี้
        var correlationId = GetOrCreateCorrelationId(context);
        
        // เก็บใน HttpContext.Items ไว้เข้าถึงใน controllers/services
        context.Items[CorrelationIdItem] = correlationId;

        // เพิ่มใน response headers สำหรับ client/downstream services
        if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
        }

        _logger.LogDebug("CorrelationId: {CorrelationId} for {Method} {Path}",
            correlationId, context.Request.Method, context.Request.Path);

        return _next(context);
    }

    /// ดึง correlation ID จาก request header หรือสร้างใหม่ถ้าไม่มี
    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        var request = context.Request;

        // ตรวจสอบว่า client ส่ง correlation ID มาหรือไม่
        if (request.Headers.TryGetValue(CorrelationIdHeader, out var existingCorrelationId))
        {
            return existingCorrelationId.ToString();
        }

        // สร้าง correlation ID ใหม่ (16 ตัวอักษร)
        return GenerateCorrelationId();
    }

    /// สร้าง correlation ID สั้นๆ จาก GUID
    private static string GenerateCorrelationId()
    {
        return Guid.NewGuid().ToString("N")[..16].ToUpperInvariant();
    }
}

/// Extension methods สำหรับใช้งาน CorrelationIdMiddleware ง่ายขึ้น
public static class CorrelationIdMiddlewareExtensions
{
    /// เพิ่ม CorrelationIdMiddleware ใน pipeline
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }

    /// ดึง correlation ID จาก HttpContext ปัจจุบัน
    public static string GetCorrelationId(this HttpContext context)
    {
        return context.Items[CorrelationIdMiddleware.CorrelationIdItem] as string ?? context.TraceIdentifier;
    }
}