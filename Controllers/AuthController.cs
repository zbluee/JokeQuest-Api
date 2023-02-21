using Microsoft.AspNetCore.Mvc;
using JokeApi.Modles;
using Bcrypt = BCrypt.Net.BCrypt;
using JokeApi.Services;

namespace JokeApi.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserServices _userServices;
    private readonly IJwtUtils _jwtUtils;

    public AuthController(IUserServices userServices, IJwtUtils jwtUtils)
    {
        _userServices = userServices;
        _jwtUtils = jwtUtils;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<User>> Register([FromBody] UserRegisterDto requestDto)
    {

        if (!ModelState.IsValid) return BadRequest(ModelState); //handle by api controller
        var user = await _userServices.FindUserByEmail(requestDto.Email);
        if (user != null) return BadRequest(new AuthResponse { Success = false, Msg = "User already exists!" });

        var newUser = new User { Name = requestDto.Name, Email = requestDto.Email, Password = Bcrypt.HashPassword(requestDto.Password) };
        await _userServices.CreateUser(newUser);
        user = await _userServices.FindUserByEmail(requestDto.Email);
        //generate token
        var token = _jwtUtils.Sign(user);
        return Ok(new Response { Success = true, Msg = "Successfully Registered", Token = token });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody] UserSignInDto requestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); //handle by api controller
        var user = await _userServices.FindUserByEmail(requestDto.Email);
        if (user == null) return Unauthorized(new AuthResponse { Success = false, Msg = "Invalid Credentials" });
        var isPasswordValid = Bcrypt.Verify(requestDto.Password, user.Password);
        if (!isPasswordValid) return Unauthorized(new AuthResponse { Success = false, Msg = "Invalid Credentials" });
        //generate token
        var token = _jwtUtils.Sign(user);
        return Ok(new Response { Success = true, Msg = "Successfully Loggedin", Token = token });
    }
}
