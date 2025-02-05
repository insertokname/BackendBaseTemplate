using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace BackendOlimpiadaIsto.application.Commands;

public record DeleteQuestionCommand(
    Guid QuestionId
)
{ }