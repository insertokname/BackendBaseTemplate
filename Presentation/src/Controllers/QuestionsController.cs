using System.Security.Claims;
using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Questions;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.application.Query.Questions;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class QuestionsController : EntityController<Question, CreateQuestionCommand>
{
    public readonly VerifyQuestionHandler _verifyHandler;
    public readonly GetRandomQuestionHandler _getRandomHandler;
    public QuestionsController(
        CreateHandler<CreateQuestionCommand, Question> createHandler,
        DeleteByIdHandler<Question> deleteHandler,
        GetAllHandler<Question> getAllHandler,
        GetRandomQuestionHandler getRandomHandler,
        VerifyQuestionHandler verifyHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
        _getRandomHandler = getRandomHandler;
        _verifyHandler = verifyHandler;
    }

    [HttpGet]
    [Route("verify")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<bool>> Verify([FromBody] VerifyQuestionCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        bool verifyResult;
        bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!isAuthenticated || !Guid.TryParse(userIdClaim, out var userId))
        {
            verifyResult = await _verifyHandler.HandleAsync(command, null);
        }
        else
        {
            verifyResult = await _verifyHandler.HandleAsync(command, userId);
        }

        return Ok(new { IsCorrect = verifyResult });
    }

    [HttpGet]
    [Route("random")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<string>> Random()
    {
        Question randomQuestion;
        bool isAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!isAuthenticated || !Guid.TryParse(userIdClaim, out var userId))
        {
            randomQuestion = await _getRandomHandler.HandleAsync(null);
        }
        else
        {
            randomQuestion = await _getRandomHandler.HandleAsync(userId);
        }
        
        return Ok(
            new
            {
                Id = randomQuestion.Id,
                QuestionPrompt = randomQuestion.QuestionPrompt,
                Answers = randomQuestion.Answers.Answers,
                QuestionSource = "Made it up"
            }
        );
    }
}
