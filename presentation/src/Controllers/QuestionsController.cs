using System.Collections.Generic;
using System.Threading.Tasks;
using BackendOlimpiadaIsto.application.Commands;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.application.Query;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly CreateQuestionHandler _createHandler;
    private readonly DeleteQuestionHandler _deleteHandler;
    private readonly GetAllQuestionsQueryHandler _getAllHandler;
    private readonly VerifyAnswerQueryHandler _verifyHandler;

    public QuestionsController(
        CreateQuestionHandler createHandler,
        DeleteQuestionHandler deleteHandler,
        GetAllQuestionsQueryHandler getAllHandler,
        VerifyAnswerQueryHandler verifyAnswerQueryHandler
    )
    {
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _verifyHandler = verifyAnswerQueryHandler;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateQuestionCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await _createHandler.HandleAsync(command);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<Question>> Delete([FromBody] DeleteQuestionCommand command)
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAll()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var query = new GetAllQuestionsQuery();
        var questions = await _getAllHandler.HandleAsync(query);
        return Ok(questions);
    }

    [HttpGet]
    [Route("verify")]
    public async Task<ActionResult<bool>> Verify([FromBody] VerifyAnswerQuery query)
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
