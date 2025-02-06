using domain.ValueObjects;

namespace BackendOlimpiadaIsto.domain.Entities;


public class Question : Entity
{
    public string QuestionPrompt { get; set; } = null!;
    public AnswerList Answers { get; set; } = null!;

    protected Question() : base(Guid.Empty) { }

    public Question(Guid id, string questionPrompt, AnswerList answers) : base(id)
    {
        QuestionPrompt = questionPrompt;
        Answers = answers;
    }
}
