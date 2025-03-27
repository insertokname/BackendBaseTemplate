using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.Users;

public class SetUserAdminCommand
{
    public required Guid UserId { get; set; }
    public required bool IsAdmin { get; set; }
};