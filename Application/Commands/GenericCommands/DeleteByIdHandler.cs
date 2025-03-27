using BackendBaseTemplate.application.Exceptions;
using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Commands.GenericCommands;


public class DeleteByIdHandler<E>
where E : Entity
{
    private readonly IRepository<E> _entityRepository;

    public DeleteByIdHandler(IRepository<E> entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task HandleAsync(DeleteByIdCommand command)
    {
        var entity = await _entityRepository.GetByIdAsync(command.Id);
        if (entity == null)
            throw new NotFoundException($"{nameof(E)} not found for the given id: {command.Id}!");
        _entityRepository.Remove(entity);
        await _entityRepository.SaveChangesAsync();
    }
}