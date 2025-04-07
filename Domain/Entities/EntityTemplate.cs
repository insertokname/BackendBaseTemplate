using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using domain.ValueObjects;

namespace BackendBaseTemplate.domain.Entities;


public class EntityTemplate : Entity
{
    public int Value { get; set; }
    public EntityTemplateDataObject EntityTemplateDataObjects { get; set; } = null!;

    protected EntityTemplate() : base(Guid.Empty) { }

    public EntityTemplate(Guid id, int value, EntityTemplateDataObject entityTemplateDataObjects) : base(id)
    {
        Value = value;
        EntityTemplateDataObjects = entityTemplateDataObjects;
    }
}
