using JokeApi.Services;
using JokeApi.Errors;
namespace JokeApi.MiddleWares;

public class JwtMiddleware : IMiddleware
{
    private readonly JwtUtils _jwtUtils;
    private readonly UserServices _userServices;
    private readonly ILogger<JwtMiddleware> _logger;


    public JwtMiddleware(JwtUtils jwtUtils, ILogger<JwtMiddleware> logger, UserServices userServices)
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
            context.Items["user"] = await _userServices.FindUserById(userId);
            // _logger.LogInformation("userId : {1}", userId);
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new BadRequestException("unable to verfiy token");
        }
    }
}