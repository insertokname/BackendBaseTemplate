using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.domain.Entities;
using domain.ValueObjects;

namespace BackendOlimpiadaIsto.application.Commands.Questions;

public class CreateQuestionCommand : ICreateCommand<Question>
{
    public required string QuestionPrompt { get; set; }
    public required List<string> Answers { get; set; }
    public required int CorrectAnswerIndex { get; set; }

    public Question CreateEntity()
    {
        return new Question(
            Guid.NewGuid(),
            QuestionPrompt,
            new AnswerList(Answers, CorrectAnswerIndex)
        );
    }
};