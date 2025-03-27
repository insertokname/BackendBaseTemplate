using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.application.Query.GenericQueries;
using BackendBaseTemplate.domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendBaseTemplate.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EntityController<E, CreateCommand> : ControllerBase
where E : Entity
where CreateCommand : ICreateCommand<E>
{
    private readonly CreateHandler<CreateCommand, E> _createHandler;
    private readonly DeleteByIdHandler<E> _deleteByIdHandler;
    private readonly GetAllHandler<E> _getAllHandler;

    public EntityController(
         CreateHandler<CreateCommand, E> createHandler,
         DeleteByIdHandler<E> deleteByIdHandler,
         GetAllHandler<E> getAllHandler
    )
    {
        _createHandler = createHandler;
        _deleteByIdHandler = deleteByIdHandler;
        _getAllHandler = getAllHandler;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<E>> Post([FromBody] CreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return Ok(await _createHandler.HandleAsync(command));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<ActionResult<Question>> Delete([FromBody] DeleteByIdCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await _deleteByIdHandler.HandleAsync(command);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAll()
    {
        return Ok(await _getAllHandler.HandleAsync());
    }

}
