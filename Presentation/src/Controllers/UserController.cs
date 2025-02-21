using System.Security.Claims;
using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Users;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.application.Query.Questions;
using BackendOlimpiadaIsto.application.Query.Users;
using BackendOlimpiadaIsto.domain.Entities;
using domain.ValueObjects;
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
    public readonly SetUserAdminHandler _setUserAdminHandler;
    public readonly GetUserByIdHandler _getUserByIdHandler;
    public readonly GetUserStatsHandler _getUserStatsHandler;
    public readonly GetAnsweredQuestionDetailsHandler _getAnsweredQuestionDetailsHandler;
    public readonly IsDailyQuestionAvailableHandler _isDailyQuestionAvailableHandler;

    public UserController(
        LoginUserHandler loginUserHandler,
        CreateUserHandler createHandler,
        DeleteByIdHandler<User> deleteHandler,
        GetAllHandler<User> getAllHandler,
        SetUserAdminHandler makeUserAdminHandler,
        GetUserByIdHandler getUserByIdHandler,
        GetUserStatsHandler getUserStatsHandler,
        GetAnsweredQuestionDetailsHandler getAnsweredQuestionDetailsHandler,
        IsDailyQuestionAvailableHandler isDailyQuestionAvailableHandler
    )
    {
        _loginUserHandler = loginUserHandler;
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _setUserAdminHandler = makeUserAdminHandler;
        _getUserByIdHandler = getUserByIdHandler;
        _getUserStatsHandler = getUserStatsHandler;
        _getAnsweredQuestionDetailsHandler = getAnsweredQuestionDetailsHandler;
        _isDailyQuestionAvailableHandler = isDailyQuestionAvailableHandler;
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
                    UsernameTakem = true
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

    [HttpGet]
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
    public async Task<ActionResult<IEnumerable<Question>>> GetAll()
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
                    AnsweredQuestion = user.AnsweredQuestions,
                    LastAnswerdQuestionStartTime = user.LastAnswerdQuestionStartTime,
                    LastAnsweredQuestionId = user.LastAnsweredQuestionId
                }
            );
        }
        return Forbid();
    }

    [Authorize]
    [HttpGet]
    [Route("IsDailyQuestionAvailable")]
    public async Task<ActionResult<UserStats>> IsDailyQuestionAvailable()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(currentUserId, out var userId))
        {
            return Ok(new
            {
                value = await _isDailyQuestionAvailableHandler.HandleAsync(userId)
            });
        }
        return Forbid();
    }

    [Authorize]
    [HttpGet]
    [Route("AnsweredQuestionDetails")]
    public async Task<ActionResult<UserStats>> GetAnsweredQuestionDetails([FromBody] GetAnsweredQuestionDetailsQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(currentUserId, out var userId))
        {
            AnsweredQuestion? answeredQuestion = await _getAnsweredQuestionDetailsHandler.HandleAsync(query, userId);
            if (answeredQuestion == null)
            {
                return Ok(new AnsweredQuestion(query.questionId, [], null));
            }
            return Ok(answeredQuestion);
        }
        return Forbid();
    }

    [Authorize]
    [HttpGet]
    [Route("Stats")]
    public async Task<ActionResult<UserStats>> GetStats()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(currentUserId, out var userId))
        {
            UserStats userStats = await _getUserStatsHandler.HandleAsync(userId);
            return Ok(userStats);
        }
        return Forbid();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
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
    public async Task<ActionResult<Question>> Delete([FromBody] DeleteByIdCommand command)
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
