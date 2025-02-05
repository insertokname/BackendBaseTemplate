using domain.ValueObjects;

namespace BackendOlimpiadaIsto.domain.Entities;


public class Question : Entity
{
    public string QuestionPrompt { get; set; }
    public AnswerList Answers { get; set; }

    protected Question() : base(Guid.Empty)
    {
        QuestionPrompt = string.Empty;
    }

    public Question(Guid id, string questionPrompt, AnswerList answers) : base(id)
    {
        QuestionPrompt = questionPrompt;
        Answers = answers;
    }
}
