using Domain.Entities;

namespace Application.Commands.GenericCommands
{
    public interface CreateCommand<E>
    where E : Entity
    {
        E CreateEntity();
    }
}