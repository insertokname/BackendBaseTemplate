namespace BackendOlimpiadaIsto.application.Commands.Questions;

public class VerifyQuestionCommand
{
    public required Guid QuestionId { get; set; }
    public required int GivenAnswerIndex { get; set; }

}