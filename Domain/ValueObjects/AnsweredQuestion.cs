namespace domain.ValueObjects;

public class AnsweredQuestion
{
    public Guid QuestionId { get; set; }
    public List<int> Attempts { get; set; } = null!;
    public DateTime? FinishedDate { get; set; } = null;
    public AnsweredQuestion()
    { }
    public AnsweredQuestion(Guid questionId, List<int> attempts, DateTime? finishedDate)
    {
        QuestionId = questionId;
        Attempts = attempts;
        FinishedDate = finishedDate;
    }

}