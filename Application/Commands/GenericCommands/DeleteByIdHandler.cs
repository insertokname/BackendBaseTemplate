using Application.Query.GenericQueries;

using Domain.Entities;

using Infrastructure.Repositories;

namespace Application.Commands.GenericCommands
{
    public class DeleteByIdHandler<E>(IRepository<E> entityRepository, GetByIdHandler<E> getByIdHandler)
    where E : Entity
    {
        public async Task HandleAsync(DeleteByIdCommand command)
        {
            var e = await getByIdHandler.HandleAsync(new GetByIdQuery { EntityId = command.EntityId });
            entityRepository.Remove(e);
            await entityRepository.SaveChangesAsync();
        }
    }
}