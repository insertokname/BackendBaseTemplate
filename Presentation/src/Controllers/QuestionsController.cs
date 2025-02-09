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
    public QuestionsController(
        CreateCommandHandler<CreateQuestionCommand, Question> createHandler,
        DeleteByIdCommandHandler<Question> deleteHandler,
        GetAllQueryHandler<Question> getAllHandler,
        VerifyQuestionHandler verifyHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
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
            return Ok(await _verifyHandler.HandleAsync(query));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
