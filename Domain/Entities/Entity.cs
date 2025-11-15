namespace Domain.Entities
{
    public class Entity(Guid id)
    {
        public Guid Id { get; set; } = id;
    }
}