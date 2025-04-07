using BackendBaseTemplate.application.Exceptions;
using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Query.EntityTemplates;


public class GetAllEntityTemplateDataObjectsHandler
{
    private readonly IRepository<EntityTemplate> _entityTemplateRepository;

    public GetAllEntityTemplateDataObjectsHandler(IRepository<EntityTemplate> entityTemplateRepository)
    {
        _entityTemplateRepository = entityTemplateRepository;
    }

    public async Task<EntityTemplateDataObject> HandleAsync(GetEntityTemplateDataObjectsQuery query)
    {
        var entityTemplates = await _entityTemplateRepository.GetByIdAsync(query.EntityTemplateId);
        if (entityTemplates == null)
        {
            throw new NotFoundException($"Couldn't find an entityTemplate with the id {query.EntityTemplateId}");
        }
        return entityTemplates.EntityTemplateDataObjects;
    }
}