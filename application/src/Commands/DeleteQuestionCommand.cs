using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace BackendOlimpiadaIsto.application.Commands;

public class DeleteQuestionCommand
{
    public required Guid QuestionId { get; set; }
};