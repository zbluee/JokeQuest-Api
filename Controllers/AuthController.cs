using Microsoft.AspNetCore.Mvc;
using JobServices.Modles;

namespace JobServices.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class AuthController : ControllerBase {

    [HttpGet("users")]
    public ActionResult<List<User>> GetAllUsers(){
        return Ok(new List<User> {new Modles.User {Name = "user", Email = "user@gmail.com", Id = "001", Password = "password"}});
    }

    [HttpPost("Register")] 
    public ActionResult Register([FromBody] UserRegisterDto requestDto){
        throw new NotImplementedException();
    }

    public ActionResult Login([FromBody] UserSignInDto requestDto) {
        throw new NotImplementedException();
    }
}
