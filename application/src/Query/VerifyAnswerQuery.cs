namespace BackendOlimpiadaIsto.application.Query;

public record VerifyAnswerQuery(
    Guid QuestionId, 
    int GivenAnswerIndex
);