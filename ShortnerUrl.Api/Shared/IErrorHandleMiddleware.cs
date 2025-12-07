namespace ShortnerUrl.Api.Shared;

public interface IErrorHandleMiddleware
{
    public Task InvokeAsync(HttpContext context);
    
    public Task HandleExceptionAsync(HttpContext context, Exception e);
    
}