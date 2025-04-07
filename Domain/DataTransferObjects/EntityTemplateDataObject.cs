namespace BackendBaseTemplate.domain.Commands.DataTransferObjects;

public class EntityTemplateDataObject
{
    public List<DateTime> TemplateData { get; set; } = null!;
    public EntityTemplateDataObject()
    { }
    public EntityTemplateDataObject(List<DateTime> template_data)
    {
        TemplateData = template_data;
    }

}