using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Entities;
using domain.ValueObjects;

namespace BackendBaseTemplate.application.Commands.Questions;

public class CreateQuestionCommand : ICreateCommand<Question>
{
    public required string QuestionPrompt { get; set; }
    public required List<string> Answers { get; set; }
    public required int CorrectAnswerIndex { get; set; }
    public required string Source { get; set; }

    public Question CreateEntity()
    {
        return new Question(
            Guid.NewGuid(),
            QuestionPrompt,
            new AnswerList(Answers, CorrectAnswerIndex),
            Source
        );
    }
};