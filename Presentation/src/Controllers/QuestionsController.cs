using System.Security.Claims;
using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.application.Commands.Questions;
using BackendBaseTemplate.application.Exceptions;
using BackendBaseTemplate.application.Query.GenericQueries;
using BackendBaseTemplate.application.Query.Questions;
using BackendBaseTemplate.domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendBaseTemplate.presentation.Controllers;
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
            try
            {
                randomQuestion = await _getRandomHandler.HandleAsync(userId);
            }
            catch (AlreadyAnsweredDailyQuestionException)
            {
                return BadRequest(new
                {
                    Error = "The daily question was already answered today!",
                    AlreadyAnsweredToday = true
                });
            }
        }
        
        return Ok(
            new
            {
                Id = randomQuestion.Id,
                QuestionPrompt = randomQuestion.QuestionPrompt,
                Answers = randomQuestion.Answers.Answers,
                QuestionSource = randomQuestion.Source
            }
        );
    }
}
