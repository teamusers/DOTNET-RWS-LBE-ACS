using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var sw = Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        var statusCode = context.Response.StatusCode;
        var duration = sw.Elapsed;

        _logger.LogInformation(
            "[HTTP] {Time} | {Status} | {Duration} | {IP} | {Method} {Path}",
            startTime.ToString("yyyy/MM/dd - HH:mm:ss"),
            statusCode,
            FormatDuration(duration),
            clientIp,
            method,
            path
        );
    }

    private string FormatDuration(TimeSpan ts)
    {
        if (ts.TotalMilliseconds < 1)
            return $"{ts.TotalMilliseconds * 1000:F1}µs";  // microseconds
        if (ts.TotalSeconds < 1)
            return $"{ts.TotalMilliseconds:F1}ms";         // milliseconds
        return $"{ts.TotalSeconds:F3}s";                   // seconds
    }
}