using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.Users;

public class CreateUserCommand
{
    public required string Username { get; set; }
    public required string Password { get; set; }
};