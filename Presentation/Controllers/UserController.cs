using System.Security.Claims;
using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.application.Commands.Users;
using BackendBaseTemplate.application.Exceptions;
using BackendBaseTemplate.application.Query.GenericQueries;
using BackendBaseTemplate.application.Query.Users;
using BackendBaseTemplate.domain.Entities;
using domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendBaseTemplate.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly LoginUserHandler _loginUserHandler;
    public readonly CreateUserHandler _createHandler;
    public readonly DeleteByIdHandler<User> _deleteHandler;
    public readonly GetAllHandler<User> _getAllHandler;
    public readonly SetUserAdminHandler _setUserAdminHandler;
    public readonly GetUserByIdHandler _getUserByIdHandler;

    public UserController(
        LoginUserHandler loginUserHandler,
        CreateUserHandler createHandler,
        DeleteByIdHandler<User> deleteHandler,
        GetAllHandler<User> getAllHandler,
        SetUserAdminHandler makeUserAdminHandler,
        GetUserByIdHandler getUserByIdHandler
    )
    {
        _loginUserHandler = loginUserHandler;
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _setUserAdminHandler = makeUserAdminHandler;
        _getUserByIdHandler = getUserByIdHandler;
    }

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserCommand command)
    {
        try
        {
            User user = await _createHandler.HandleAsync(command);
            return Ok(user);
        }
        catch (UsernameTakenException)
        {
            return BadRequest(
                new
                {
                    Error = "Username was already taken!",
                    UsernameTaken = true
                }
            );

        }
        catch (InvalidUsernameCharacterException)
        {
            return BadRequest(
                new
                {
                    Error = "Username contains invalid characters!",
                    InvalidUsername = true
                }
            );
        }
        catch (UsernameTooLongException)
        {
            return BadRequest(
                new
                {
                    Error = "Username is too long!",
                    UsernameTooLong = true
                }
            );
        }
    }

    [HttpPost]
    [Route("login")]
    [EnableRateLimiting("LoginRateLimit")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            string token = await _loginUserHandler.HandleAsync(command);
            return Ok(new { Token = token });
        }
        catch (BadCredentialsException)
        {
            return BadRequest(new { Error = "Given bad username or password!" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return Ok(await _getAllHandler.HandleAsync());
    }

    [Authorize]
    [HttpGet]
    [Route("Self")]
    public async Task<ActionResult<object>> GetSelf()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(currentUserId, out var userId))
        {
            User user = await _getUserByIdHandler.HandleAsync(new GetUserByIdQuery { UserId = userId });
            return Ok(
                new
                {
                    Id = user.Id,
                    Username = user.Username,
                }
            );
        }
        return Forbid();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("ById")]
    public async Task<ActionResult<User>> GetById([FromBody] GetUserByIdQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(await _getUserByIdHandler.HandleAsync(query));
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteByIdCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if ((User?.IsInRole("Admin") ?? false) || (currentUserId == command.Id.ToString()))
        {
            Console.WriteLine("Deleting");
            await _deleteHandler.HandleAsync(command);
            return Ok();
        }
        else
        {
            return Forbid();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("SetAdmin")]
    public async Task<ActionResult> SetAdmin([FromBody] SetUserAdminCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await _setUserAdminHandler.HandleAsync(command);
        return Ok();
    }
}
