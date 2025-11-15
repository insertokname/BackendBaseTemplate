using Application.Exceptions;

using Domain.Entities;

using Infrastructure.Repositories;

namespace Application.Query.GenericQueries
{
    public class GetByIdHandler<E>(IRepository<E> entityRepository)
    where E : Entity
    {
        public async Task<E> HandleAsync(GetByIdQuery query)
        {
            var e = await entityRepository.GetByIdAsync(query.EntityId);
            if (e == null)
            {
                throw new NotFoundException($"No entity of type {typeof(E)}: found by id {query.EntityId}.");
            }
            return e;
        }
    }
}