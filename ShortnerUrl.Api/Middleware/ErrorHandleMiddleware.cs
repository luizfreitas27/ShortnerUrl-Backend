using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using ShortnerUrl.Api.Exeptions;
using ShortnerUrl.Api.Shared;

namespace ShortnerUrl.Api.Middleware;

public class ErrorHandleMiddleware : IErrorHandleMiddleware
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ErrorHandleMiddleware> _logger;
    
    public ErrorHandleMiddleware(
        RequestDelegate next, 
        IWebHostEnvironment env, 
        ILogger<ErrorHandleMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    public async Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning(
                "The response already has been modified. Exception: {Message}", 
                e.Message);
            return;
        }

        var response = context.Response;

        if (e is OperationCanceledException or TaskCanceledException)
        {
            if (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Request canceled by the client: {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);
                
                response.StatusCode = 499;
                return;
            }
            
            _logger.LogWarning("The operation was canceled: {Message}", e.Message);
            response.StatusCode = (int)HttpStatusCode.ServiceUnavailable; // 503
            response.ContentType = "application/json";
            
            var timeoutResponse = JsonSerializer.Serialize(
                new { IsSuccess = false, Error = "Timeout exceeded.", Code = 503 },
                _jsonOptions);

            await response.WriteAsync(timeoutResponse, context.RequestAborted);
            return;
        }
        
        response.ContentType = "application/json";

        var status = e switch
        {
            AppException appEx => (HttpStatusCode)appEx.StatusCode,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        if (status == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(e, "Internal error: {Message}", e.Message);
        }
        else
        {
            _logger.LogWarning("Application error: {StatusCode} - {Message}", (int)status, e.Message);
        }

        if (_env.IsDevelopment())
        {
            var devResult = JsonSerializer.Serialize(new
            {
                IsSuccess = false,
                Error = e.Message,
                Code = (int)status,
                Inner = e.InnerException?.Message,
                Stack = e.StackTrace,
                InnerStack = e.InnerException?.StackTrace
            }, _jsonOptions);

            response.StatusCode = (int)status;
            await response.WriteAsync(devResult, context.RequestAborted);
            return;
        }

        var result = JsonSerializer.Serialize(
            new { IsSuccess = false, Error = e.Message, Code = (int)status },
            _jsonOptions);

        response.StatusCode = (int)status;

        await response.WriteAsync(result, context.RequestAborted);
    }
}