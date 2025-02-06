using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Questions;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EntityController<E, CreateCommand> : ControllerBase
where E : Entity
where CreateCommand : ICreateCommand<E>
{
    private readonly CreateCommandHandler<CreateCommand, E> _createHandler;
    private readonly DeleteByIdCommandHandler<E> _deleteHandler;
    private readonly GetAllQueryHandler<E> _getAllHandler;

    public EntityController(
         CreateCommandHandler<CreateCommand, E> createHandler,
         DeleteByIdCommandHandler<E> deleteHandler,
         GetAllQueryHandler<E> getAllHandler
    )
    {
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpPost]
    public async Task<ActionResult<E>> Post([FromBody] CreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return Ok(await _createHandler.HandleAsync(command));
    }

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAll()
    {
        return Ok(await _getAllHandler.HandleAsync());
    }

}
