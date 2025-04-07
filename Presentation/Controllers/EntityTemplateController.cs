using BackendBaseTemplate.application.Commands.EntityTemplates;
using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.application.Query.EntityTemplates;
using BackendBaseTemplate.application.Query.GenericQueries;
using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using BackendBaseTemplate.domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendBaseTemplate.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EntityTemplateController : EntityController<EntityTemplate, CreateEntityTemplateCommand>
{
    private readonly GetRandomHandler<EntityTemplate> _getRandomHandler;
    private readonly GetAllEntityTemplateDataObjectsHandler _getEntityTemplateDataObjectsHandler;
    private readonly SetNewTemplateDataObjectHandler _setNewTemplateDataObjectHandler;
    public EntityTemplateController(
        CreateHandler<CreateEntityTemplateCommand, EntityTemplate> createHandler,
        DeleteByIdHandler<EntityTemplate> deleteHandler,
        GetAllHandler<EntityTemplate> getAllHandler,
        GetRandomHandler<EntityTemplate> getRandomHandler,
        GetAllEntityTemplateDataObjectsHandler getAllEntityTemplateDataObjectsHandler,
        SetNewTemplateDataObjectHandler setNewTemplateDataObjectHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
        _getRandomHandler = getRandomHandler;
        _getEntityTemplateDataObjectsHandler = getAllEntityTemplateDataObjectsHandler;
        _setNewTemplateDataObjectHandler = setNewTemplateDataObjectHandler;
    }

    [HttpGet]
    [Route("random")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<int>> Random()
    {
        return Ok(
            (await _getRandomHandler.HandleAsync()).Value
        );
    }

    [HttpGet]
    [Route("DataObjects")]
    public async Task<ActionResult<EntityTemplateDataObject>> GetEntityTemplateDataObject(GetEntityTemplateDataObjectsQuery query)
    {
        return Ok(
            await _getEntityTemplateDataObjectsHandler.HandleAsync(query)
        );
    }

    [HttpPost]
    [Route("SetDataObject")]
    public async Task<ActionResult> SetEntityTemplateDataObject(SetNewTemplateDataObjectCommand command)
    {
        await _setNewTemplateDataObjectHandler.HandleAsync(command);
        return Ok();
    }
}
