using domain.ValueObjects;

namespace BackendOlimpiadaIsto.domain.Entities;


public class Question : Entity
{
    public string QuestionPrompt { get; set; } = null!;
    public AnswerList Answers { get; set; } = null!;
    public string Source { get; set; } = null!;

    protected Question() : base(Guid.Empty) { }

    public Question(Guid id, string questionPrompt, AnswerList answers, string source) : base(id)
    {
        Source = source;
        QuestionPrompt = questionPrompt;
        Answers = answers;
    }
}
