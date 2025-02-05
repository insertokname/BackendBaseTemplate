using System.Reflection.Metadata;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;

namespace BackendOlimpiadaIsto.application.Query;

public class VerifyAnswerQueryHandler
{
    private readonly IRepository<Question> _questionRepository;


    public VerifyAnswerQueryHandler(IRepository<Question> repository)
    {
        _questionRepository = repository;
    }

    public async Task<bool> HandleAsync(VerifyAnswerQuery query)
    {
        var question = await _questionRepository.GetByIdAsync(query.QuestionId);
        if (question == null)
            throw new ArgumentException("Question not found for the given id", nameof(query.QuestionId));
        return question.Answers.CorrectAnswerIndex == query.GivenAnswerIndex;
    }
}