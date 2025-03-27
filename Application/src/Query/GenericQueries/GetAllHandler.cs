using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Query.GenericQueries;


public class GetAllHandler<E>
where E : Entity
{
    private readonly IRepository<E> _entityRepository;

    public GetAllHandler(IRepository<E> entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task<IEnumerable<E>> HandleAsync()
    {
        return await _entityRepository.GetAllAsync();
    }
}