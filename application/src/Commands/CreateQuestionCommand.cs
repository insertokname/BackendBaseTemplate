namespace BackendOlimpiadaIsto.application.Commands;

public record CreateQuestionCommand(
    string QuestionPrompt,
    List<string> Answers,
    int CorrectAnswerIndex
);