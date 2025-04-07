using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.application.Exceptions;
using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Commands.EntityTemplates;

public class SetNewTemplateDataObjectHandler
{
    private readonly IRepository<EntityTemplate> _entityTemplateRepository;

    public SetNewTemplateDataObjectHandler(IRepository<EntityTemplate> entityTemplateRepository)
    {
        _entityTemplateRepository = entityTemplateRepository;
    }

    public async Task HandleAsync(SetNewTemplateDataObjectCommand command)
    {
        var entityTemplate = await _entityTemplateRepository.GetByIdAsync(command.EntityTemplateGuid);

        if (entityTemplate == null)
        {
            throw new NotFoundException($"No TemplateEntity found by the id {command.EntityTemplateGuid}");
        }
        entityTemplate.EntityTemplateDataObjects = command.EntityTemplateDataObject;
        await _entityTemplateRepository.SaveChangesAsync();
    }
}