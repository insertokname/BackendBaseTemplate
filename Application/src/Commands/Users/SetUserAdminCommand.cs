using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.application.Commands.Users;

public class SetUserAdminCommand
{
    public required Guid UserId { get; set; }
    public required bool IsAdmin { get; set; }
};