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
    private readonly CreateCommandHandler<CreateCommand, E> _createCommandHandler;
    private readonly DeleteByIdCommandHandler<E> _deleteByIdCommandHandler;
    private readonly GetAllQueryHandler<E> _getAllQueryHandler;

    public EntityController(
         CreateCommandHandler<CreateCommand, E> createCommandHandler,
         DeleteByIdCommandHandler<E> deleteByIdCommandHandler,
         GetAllQueryHandler<E> getAllQueryHandler
    )
    {
        _createCommandHandler = createCommandHandler;
        _deleteByIdCommandHandler = deleteByIdCommandHandler;
        _getAllQueryHandler = getAllQueryHandler;
    }

    [HttpPost]
    public async Task<ActionResult<E>> Post([FromBody] CreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return Ok(await _createCommandHandler.HandleAsync(command));
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
            await _deleteByIdCommandHandler.HandleAsync(command);
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
        return Ok(await _getAllQueryHandler.HandleAsync());
    }

}
