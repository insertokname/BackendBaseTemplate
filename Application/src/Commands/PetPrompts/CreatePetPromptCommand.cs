using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.application.Commands.PetPrompts;

public class CreatePetPromptCommand : ICreateCommand<PetPrompt>
{
    public required string Prompt { get; set; }
    public PetPrompt CreateEntity()
    {
        return new PetPrompt(Guid.NewGuid(), Prompt);
    }
};
