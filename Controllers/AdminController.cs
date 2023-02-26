using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JokeApi.Modles;
using JokeApi.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CustomRoles.Admin)]
[ApiController]
[Route("api/admin")]
public class AdministrationController : ControllerBase
{
    private readonly IUserServices _userServices;
    private readonly IJokeServices _jokeServices;
    public AdministrationController(IUserServices userServices, IJokeServices jokeServices)
    {
        _userServices = userServices;
        _jokeServices = jokeServices;
    }

    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<User>>> GetAllUsers() => Ok(await _userServices.GetAllAsync());

    [HttpGet("/api/jokes"), AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<User>>> GetAllJokes() => Ok(await _jokeServices.GetAllJokes());

    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) return BadRequest();
        var user = await _userServices.FindUserById(id);
        if (user == null) return NotFound(new AuthResponse { Success = false, Msg = String.Format("No user with id : {0} found", id) });
        return Ok(user);
    }

    [HttpGet("joke/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetJoke(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) return BadRequest();
        var joke = await _jokeServices.FindJokeById(id);
        if (joke == null) return NotFound(new AuthResponse { Success = false, Msg = String.Format("No joke with id : {0} found", id) });
        return Ok(joke);
    }

    [HttpDelete("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) return BadRequest();
        var user = await _userServices.DeleteOneAsync(id);
        if (user == null) return NotFound(new AuthResponse { Success = false, Msg = String.Format("No user with id : {0} found", id) });

        return Ok(new AuthResponse { Success = true, Msg = "Successfully Deleted" });
    }

    [HttpDelete("joke/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteJoke(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) return BadRequest();
        var joke = await _jokeServices.DeleteOneById(id);
        if (joke == null) return NotFound(new AuthResponse { Success = false, Msg = String.Format("No joke with id : {0} found", id) });

        return Ok(new AuthResponse { Success = true, Msg = "Successfully Deleted" });
    }

    [HttpDelete("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAllUsers()
    {
        await _userServices.DeleteAllAsync();
        return Ok();
    }

    [HttpDelete("jokes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteAllJokes()
    {
        await _jokeServices.DeleteAll();
        return Ok();
    }

}


