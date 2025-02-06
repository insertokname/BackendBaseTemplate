namespace BackendOlimpiadaIsto.application.Query.Questions;

public class VerifyQuestionQuery
{
    public required Guid QuestionId { get; set; }
    public required int GivenAnswerIndex { get; set; }

}