using Microsoft.AspNetCore.Mvc;
using JobServices.Modles;

namespace JobServices.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class AuthController : ControllerBase
{

    private readonly UserServices _userServices;
    public AuthController(UserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<User>>> GetAllUsers() => await _userServices.GetAllAsync();

    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register([FromBody] UserRegisterDto requestDto)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (_userServices.FindUserByEmail(requestDto.Email) != null) return BadRequest();
        var newUser = new User { Name = requestDto.Name, Email = requestDto.Email, Password = requestDto.Password };
        await _userServices.CreateUser(newUser);
        var user = _userServices.FindUserByEmail(requestDto.Email);
        return Ok(user);
    }

    // public ActionResult Login([FromBody] UserSignInDto requestDto) {
    //     throw new NotImplementedException();
    // }
}
