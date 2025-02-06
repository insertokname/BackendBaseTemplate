using System.Reflection.Metadata;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;

namespace BackendOlimpiadaIsto.application.Query.Questions;

public class VerifyQuestionHandler
{
    private readonly IRepository<Question> _questionRepository;


    public VerifyQuestionHandler(IRepository<Question> repository)
    {
        _questionRepository = repository;
    }

    public async Task<bool> HandleAsync(VerifyQuestionQuery query)
    {
        var question = await _questionRepository.GetByIdAsync(query.QuestionId);
        if (question == null)
            throw new NotFoundException($"Question not found for the given id: {query.QuestionId}");
        return question.Answers.CorrectAnswerIndex == query.GivenAnswerIndex;
    }
}