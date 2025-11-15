using Domain.Entities;

using Infrastructure.Repositories;

namespace Application.Query.GenericQueries
{
    public class GetAllHandler<E>(IRepository<E> entityRepository)
    where E : Entity
    {
        public async Task<IEnumerable<E>> HandleAsync()
        {
            return await entityRepository.GetAllAsync();
        }
    }
}