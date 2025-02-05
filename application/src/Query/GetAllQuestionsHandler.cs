using BackendOlimpiadaIsto.application.Query;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

public class GetAllQuestionsQueryHandler
{
    private readonly IRepository<Question> _questionRepository;

    public GetAllQuestionsQueryHandler(IRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<IEnumerable<Question>> HandleAsync(GetAllQuestionsQuery query)
    {
        return await _questionRepository.GetAllAsync();
    }
}