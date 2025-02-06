using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.application.Query.PetPrompts;

public class GetRandomPromptQueryHandler
{
    private readonly IRepository<PetPrompt> _petPromptRepository;

    public GetRandomPromptQueryHandler(IRepository<PetPrompt> petPomptRepository)
    {
        _petPromptRepository = petPomptRepository;
    }

    public async Task<PetPrompt> HandleAsync()
    {
        var queryable = _petPromptRepository.GetQueryable();
        int count = await queryable.CountAsync();
        if (count == 0)
            throw new NotFoundException("Cannot find any PetPrompts!");

        Random random = new Random();
        int randomIndex = random.Next(count);

        var randomPrompt = await queryable.Skip(randomIndex).FirstOrDefaultAsync();
        if (randomPrompt == null)
            throw new NotFoundException("Cannot find any PetPrompts!");

        return randomPrompt;
    }

}