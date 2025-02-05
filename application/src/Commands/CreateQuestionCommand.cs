namespace BackendOlimpiadaIsto.application.Commands;

public class CreateQuestionCommand
{
    public required string QuestionPrompt { get; set; }
    public required List<string> Answers { get; set; }
    public required int CorrectAnswerIndex { get; set; }
};