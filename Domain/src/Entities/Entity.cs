namespace BackendOlimpiadaIsto.domain.Entities;

public class Entity
{
    public Guid Id { get; set; }

    public Entity(Guid id)
    {
        Id = id;
    }
}