using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.application.Query.GenericQueries;

public class GetRandomHandler<E>
where E: Entity
{
    private readonly IRepository<E> _entityRepository;

    public GetRandomHandler(IRepository<E> entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task<E> HandleAsync()
    {
        var queryable = _entityRepository.GetQueryable();
        int count = await queryable.CountAsync();
        if (count == 0)
            throw new NotFoundException($"Cannot find any Entities of type {typeof(E).Name}!");

        Random random = new Random();
        int randomIndex = random.Next(count);

        var randomEntity = await queryable.Skip(randomIndex).FirstOrDefaultAsync();
        if (randomEntity == null)
            throw new NotFoundException($"Cannot find any Entities of type {typeof(E).Name}!");

        return randomEntity;
    }

}