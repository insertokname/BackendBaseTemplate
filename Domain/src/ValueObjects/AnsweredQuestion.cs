namespace domain.ValueObjects;

public class AnsweredQuestion
{
    public Guid QuestionId { get; set; }
    public List<int> Attempts { get; set; } = null!;
    public bool IsFinished { get; set; }

    public AnsweredQuestion() { }

    public AnsweredQuestion(Guid questionId, List<int> attempts, bool isFinished)
    {
        QuestionId = questionId;
        Attempts = attempts;
        IsFinished = isFinished;
    }

}