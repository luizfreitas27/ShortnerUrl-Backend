using System.Net;
using System.Net.Mime;
using System.Text.Json;
using ShortnerUrl.Api.Exeptions;
using ShortnerUrl.Api.Shared;

namespace ShortnerUrl.Api.Middleware;

public class ErrorHandleMiddleware : IErrorHandleMiddleware
{
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
            
            var timeoutResponse = JsonSerializer.Serialize(new
            {
                success = false,
                error = "Timout exceed.",
                code = 503
            });
            
            await response.WriteAsync(timeoutResponse);
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

        var errorResponse = new Dictionary<string, object?>
        {
            { "success", false },
            { "error", e.Message },
            { "code", (int)status }
        };

        if (_env.IsDevelopment())
        {
            errorResponse["inner"] = e.InnerException?.Message;
            errorResponse["stack"] = e.StackTrace;
            
            if (e.InnerException != null)
            {
                errorResponse["innerStack"] = e.InnerException.StackTrace;
            }
        }

        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        response.StatusCode = (int)status;

        await response.WriteAsync(result);
    }
}