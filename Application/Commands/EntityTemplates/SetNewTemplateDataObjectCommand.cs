using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Commands.EntityTemplates;

public class SetNewTemplateDataObjectCommand
{
    public required Guid EntityTemplateGuid { get; set; }
    public required EntityTemplateDataObject EntityTemplateDataObject { get; set; }
};
