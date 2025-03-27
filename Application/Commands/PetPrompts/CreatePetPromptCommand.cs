using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.PetPrompts;

public class CreatePetPromptCommand : ICreateCommand<PetPrompt>
{
    public required string Prompt { get; set; }
    public PetPrompt CreateEntity()
    {
        return new PetPrompt(Guid.NewGuid(), Prompt);
    }
};
