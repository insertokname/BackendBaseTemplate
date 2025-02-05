using BackendOlimpiadaIsto.application.Commands;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

public class DeleteQuestionHandler
{
    private readonly IRepository<Question> _questionRepository;

    public DeleteQuestionHandler(IRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task HandleAsync(DeleteQuestionCommand command)
    {
        var question = await _questionRepository.GetByIdAsync(command.QuestionId);
        if (question == null)
            throw new NotFoundException($"Question not found for the given id: {command.QuestionId}");
        _questionRepository.Remove(question);
        await _questionRepository.SaveChangesAsync();
    }
}