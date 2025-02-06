using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.application.Commands.GenericCommands;

public interface ICreateCommand<E>
where E : Entity
{
    public E CreateEntity();
}