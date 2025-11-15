
using Domain.Entities;

using Infrastructure.Repositories;

namespace Application.Commands.GenericCommands
{
    public class CreateHandler<CreateCommand, E>(IRepository<E> entityRepository)
    where CreateCommand : CreateCommand<E>
    where E : Entity
    {
        public async Task<E> HandleAsync(CreateCommand command)
        {
            E newEntity = command.CreateEntity();
            await entityRepository.AddAsync(newEntity);
            await entityRepository.SaveChangesAsync();
            return newEntity;
        }
    }
}