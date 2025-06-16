using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RWS_LBE_ACS.Data;
using RWS_LBE_ACS.Models;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AppDbContext db)
    {
        var stopwatch = Stopwatch.StartNew();

        // Capture request body
        context.Request.EnableBuffering();
        var requestBody = await ReadStreamAsync(context.Request.Body);
        context.Request.Body.Position = 0;

        // Capture response body
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context); // Proceed to controller

        stopwatch.Stop();

        // Read response body
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        // Restore original response stream
        await responseBody.CopyToAsync(originalBodyStream);

        // Get AppID from context or header
        var actorId = context.Items["app_id"]?.ToString() ?? context.Request.Headers["AppID"].ToString();

        Console.WriteLine($"[AUDIT] {context.Request.Method} {context.Request.Path} logged");


        // Create audit entry
        var auditLog = new AuditLog
        {
            ActorID = actorId,
            Method = context.Request.Method,
            Path = context.Request.Path,
            StatusCode = context.Response.StatusCode,
            ClientIP = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"],
            RequestBody = requestBody,
            ResponseBody = responseText,
            LatencyMs = stopwatch.ElapsedMilliseconds,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            db.AuditLogs.Add(auditLog);
            await db.SaveChangesAsync(); // NO Task.Run here
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuditLogMiddleware] DB error: {ex}");
        }
    }

    private async Task<string> ReadStreamAsync(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }
}
