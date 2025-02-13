using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Questions;
using BackendOlimpiadaIsto.application.Exceptions;
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
    public readonly GetRandomQueryHandler<Question> _getRandomQueryHandler;
    public QuestionsController(
        CreateCommandHandler<CreateQuestionCommand, Question> createHandler,
        DeleteByIdCommandHandler<Question> deleteHandler,
        GetAllQueryHandler<Question> getAllHandler,
        GetRandomQueryHandler<Question> getRandomQueryHandler,
        VerifyQuestionHandler verifyHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
        _getRandomQueryHandler = getRandomQueryHandler;
        _verifyHandler = verifyHandler;
    }

    [HttpGet]
    [Route("verify")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<bool>> Verify([FromBody] VerifyQuestionQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try
        {
	    var verify = await _verifyHandler.HandleAsync(query);
            return Ok(new {IsCorrect = verify});
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("random")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<string>> Random()
    {
        var randomQuestion = await _getRandomQueryHandler.HandleAsync();
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
