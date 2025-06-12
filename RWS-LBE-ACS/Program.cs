using Microsoft.EntityFrameworkCore;
using RWS_LBE_ACS.Data;
using RWS_LBE_ACS.Services;
using RWS_LBE_ACS.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// 1) Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

//Conditionally run the JWT middleware for "/api/v1/send/**" only
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api/v1/send", StringComparison.OrdinalIgnoreCase),
    sendBranch =>
    {
        sendBranch.UseMiddleware<JwtInterceptorMiddleware>();
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var secretBase64 = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(secretBase64))
    throw new InvalidOperationException("Missing Jwt:Secret in configuration");

byte[] keyBytes;
try
{
    keyBytes = Convert.FromBase64String(secretBase64);
}
catch
{
    throw new InvalidOperationException("Jwt:Secret must be a Base64-encoded string");
}

TokenInterceptor.SetJwtSecret(keyBytes);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
