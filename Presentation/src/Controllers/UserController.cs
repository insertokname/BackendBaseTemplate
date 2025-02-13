using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Users;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly LoginUserHandler _loginUserHandler;
    public readonly CreateUserHandler _createHandler;
    public readonly DeleteByIdHandler<User> _deleteHandler;
    public readonly GetAllHandler<User> _getAllHandler;
    public UserController(
        LoginUserHandler loginUserHandler,
        CreateUserHandler createHandler,
        DeleteByIdHandler<User> deleteHandler,
        GetAllHandler<User> getAllHandler
    )
    {
        _loginUserHandler = loginUserHandler;
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserCommand command)
    {
        CreateUserResult result = await _createHandler.HandleAsync(command);
        switch (result)
        {
            case CreateUserResult.Ok newUser:
                return Ok(newUser);
            case CreateUserResult.UsernameTaken:
                return BadRequest("Username was already taken!");
            default:
                throw new Exception("Unexpected CreateUserResult value!");
        }
    }

    [HttpGet]
    [Route("login")]
    [EnableRateLimiting("LoginRateLimit")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
    {
        switch (await _loginUserHandler.HandleAsync(command))
        {
            case LoginUserResult.Ok token:
                return Ok(token.Token);
            case LoginUserResult.IncorectPassword:
                return Ok("Bad Password");
            case LoginUserResult.NoUsernameFound:
                return Ok("Bad Username");
        }
        return Ok("Unknown error occured");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAll()
    {
        return Ok(await _getAllHandler.HandleAsync());
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<Question>> Delete([FromBody] DeleteByIdCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
            await _deleteHandler.HandleAsync(command);
            return Ok();
    }
}
