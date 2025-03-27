using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.GenericCommands;

public interface ICreateCommand<E>
where E : Entity
{
    public E CreateEntity();
}