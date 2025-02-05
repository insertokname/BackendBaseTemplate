using BackendOlimpiadaIsto.application.Commands;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

public class CreateQuestionHandler
{
    private readonly IRepository<Question> _questionRepository;

    public CreateQuestionHandler(IRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task HandleAsync(CreateQuestionCommand command)
    {
        var answerList = new AnswerList(command.Answers, command.CorrectAnswerIndex);

        var question = new Question(Guid.NewGuid(), command.QuestionPrompt, answerList);

        await _questionRepository.AddAsync(question);
        await _questionRepository.SaveChangesAsync();
    }
}