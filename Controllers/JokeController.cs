using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using JokeApi.Modles;
using JokeApi.Services;
using System.Linq;

namespace JokeApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CustomRoles.Member)]
[ApiController]
[Route("api/[Controller]")]
public class JokeController : ControllerBase {

    private readonly IJokeServices _jokeServices;
    private readonly ILogger<JokeController> _logger;

    public JokeController(IJokeServices jokeServices, ILogger<JokeController> logger) {
        _jokeServices = jokeServices;
        _logger = logger;
        }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllJokes([FromQuery] QueryParameters queryParams) {
        var userId = HttpContext.Items["userId"];
        if(userId == null) throw new ArgumentNullException("js");
        var pageSize = queryParams.PageSize < 1 ?  10 : queryParams.PageSize;
        var page = queryParams.Page < 1 ? 1 : queryParams.Page;
        var jokes = await _jokeServices.GetAllAsync(userId : userId.ToString(), pageSize: pageSize, page : page);
        if(!String.IsNullOrWhiteSpace(queryParams.Title)) jokes = jokes.FindAll(joke => joke.JokeQuestion.Contains(queryParams.Title, StringComparison.CurrentCultureIgnoreCase));
        return Ok(new Res{Success = true, Joke = jokes, Count = jokes.Count()});
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateJoke([FromBody] JokeDto jokeDto){

        var userId = HttpContext.Items["userId"];
        if(userId == null) throw new ArgumentNullException("userId");
        // _logger.LogInformation("{0}\n{1}\n{2}", jokeDto.JokeQuestion, jokeDto.JokeAnswer, userId.ToString());
        var joke = new Joke {JokeQuestion = jokeDto.JokeQuestion, JokeAnswer = jokeDto.JokeAnswer, CreatedBy = userId.ToString()};
        await _jokeServices.CreateOneAsync(joke);
        return CreatedAtAction(nameof(GetAllJokes), new {id = "ate"}, jokeDto);
        
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Joke>> GetJoke(string id) { 
        if(String.IsNullOrWhiteSpace(id)) return BadRequest();
        var userId = HttpContext.Items["userId"];
        if(userId == null) throw new ArgumentNullException("userId");
        var joke = await _jokeServices.GetOneAsync(userId.ToString(), id);
        if(joke == null) return NotFound(new AuthResponse { Success = false, Msg = String.Format("No joke with id : {0} found", id) });
        
        return Ok(joke);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Joke>> UpdateJoke([FromBody] JokeDto jokeDto, string id) {
        if(jokeDto == null || String.IsNullOrWhiteSpace(id)) return BadRequest();
        var userId = HttpContext.Items["userId"];
        if(userId == null) throw new ArgumentNullException("userId");
        var joke = new Joke {Id = id, JokeQuestion = jokeDto.JokeQuestion, JokeAnswer = jokeDto.JokeAnswer, CreatedBy = userId.ToString()};
        var update = await _jokeServices.UpdateJoke(userId.ToString(), id, joke);

        return Ok(update);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteJoke(string id) {
        if(String.IsNullOrWhiteSpace(id)) return BadRequest();
        var userId = HttpContext.Items["userId"];
        if(userId == null) throw new ArgumentNullException("userId");
        var joke = await _jokeServices.DeleteOne(userId.ToString(), id);
        if(joke == null) return NotFound(new AuthResponse { Success = false, Msg = String.Format("No joke with id : {0} found", id) });
        return Ok(new AuthResponse { Success = true, Msg = "Successfully Deleted"});
    }
}