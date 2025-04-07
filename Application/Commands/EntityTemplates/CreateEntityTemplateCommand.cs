using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.EntityTemplates;

public class CreateEntityTemplateCommand : ICreateCommand<EntityTemplate>
{
    public required int Value { get; set; }
    public required EntityTemplateDataObject EntityTemplateDataObject { get; set; }
    public EntityTemplate CreateEntity()
    {
        return new EntityTemplate(Guid.NewGuid(), Value, EntityTemplateDataObject);
    }
};
