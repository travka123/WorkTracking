using UseCases.UseCases;

namespace WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UseCaseExeption ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (BadHttpRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("invalid request format");
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
        }
    }
}
