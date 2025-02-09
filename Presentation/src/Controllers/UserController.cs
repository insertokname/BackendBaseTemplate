using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Users;
using BackendOlimpiadaIsto.application.Exceptions;
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
    public readonly LoginUserCommandHandler _loginUserCommandHandler;
    public readonly CreateUserCommandHandler _createHandler;
    public readonly DeleteByIdCommandHandler<User> _deleteHandler;
    public readonly GetAllQueryHandler<User> _getAllHandler;
    public UserController(
        LoginUserCommandHandler loginUserCommandHandler,
        CreateUserCommandHandler createHandler,
        DeleteByIdCommandHandler<User> deleteHandler,
        GetAllQueryHandler<User> getAllHandler
    )
    {
        _loginUserCommandHandler = loginUserCommandHandler;
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
        switch (await _loginUserCommandHandler.HandleAsync(command))
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
        try
        {
            await _deleteHandler.HandleAsync(command);
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
