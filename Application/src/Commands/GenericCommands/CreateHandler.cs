using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Commands.GenericCommands;

public class CreateHandler<CreateCommand, E>
where CreateCommand : ICreateCommand<E>
where E : Entity
{
    private readonly IRepository<E> _entityRepository;

    public CreateHandler(IRepository<E> entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task<E> HandleAsync(CreateCommand command)
    {
        E newEntity = command.CreateEntity();
        await _entityRepository.AddAsync(newEntity);
        await _entityRepository.SaveChangesAsync();
        return newEntity;
    }
}