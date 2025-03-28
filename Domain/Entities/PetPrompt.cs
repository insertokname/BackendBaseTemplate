namespace BackendBaseTemplate.domain.Entities;


public class PetPrompt : Entity
{
    public string Prompt { get; set; } = null!;

    protected PetPrompt() : base(Guid.Empty) { }

    public PetPrompt(Guid id, string prompt) : base(id)
    {
        Prompt = prompt;
    }
}
