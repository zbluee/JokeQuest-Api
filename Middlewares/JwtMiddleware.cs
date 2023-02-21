using JokeApi.Services;
using JokeApi.Errors;
namespace JokeApi.MiddleWares;

public class JwtMiddleware : IMiddleware
{
    private readonly IJwtUtils _jwtUtils;
    private readonly IUserServices _userServices;
    private readonly ILogger<JwtMiddleware> _logger;


    public JwtMiddleware(IJwtUtils jwtUtils, ILogger<JwtMiddleware> logger, IUserServices userServices)
    {
        _jwtUtils = jwtUtils;
        _logger = logger;
        _userServices = userServices;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(String.IsNullOrWhiteSpace(token)) throw new BadRequestException("Empty Token");
            var userId = _jwtUtils.Verify(token);
            //attach user to context on successful jwt validation
            context.Items["userId"] = userId;
            // _logger.LogInformation("userId : {0} {1} {2}", user.Id, user.Name, user.Email);
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new BadRequestException("problem occur with your login.");
        }
    }
}