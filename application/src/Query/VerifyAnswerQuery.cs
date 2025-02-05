using System.ComponentModel.DataAnnotations;

namespace BackendOlimpiadaIsto.application.Query;

public class VerifyAnswerQuery
{
    public required Guid QuestionId { get; set; }
    public required int GivenAnswerIndex { get; set; }

}