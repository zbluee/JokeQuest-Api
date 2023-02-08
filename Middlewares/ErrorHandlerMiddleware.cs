using System.Net;
using JokeApi.Errors;
using JokeApi.Modles;

namespace JokeApi.MiddleWares;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _looger;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
    {
        _looger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {  
            await next(context);
        }
        catch (Exception e)
        {
            _looger.LogError(e, e.Message);
            var res = new ErrorResponse {Success = false, Msg = "something weng wrong please try again later.", StatusCode = (int) HttpStatusCode.InternalServerError};
            if(e is CustomAPIException)  {res.Msg = e.Message; res.StatusCode = e.HResult;};
            if(e is FormatException) {res.Msg= e.Message; res.StatusCode = (int) HttpStatusCode.InternalServerError;};
            context.Response.StatusCode = res.StatusCode;
            await context.Response.WriteAsJsonAsync(res);
        }
    }
}