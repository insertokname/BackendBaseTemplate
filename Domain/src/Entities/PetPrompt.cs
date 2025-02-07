using domain.ValueObjects;

namespace BackendOlimpiadaIsto.domain.Entities;


public class PetPrompt : Entity
{
    public string Prompt { get; set; } = null!;

    private PetPrompt() : base(Guid.Empty) { }

    public PetPrompt(Guid id, string prompt) : base(id)
    {
        Prompt = prompt;
    }
}
