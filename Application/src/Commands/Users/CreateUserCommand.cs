using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.Users;

public class CreateUserCommand : ICreateCommand<User>
{
    public required string Username { get; set; }
    public required string Password { get; set; }

    public User CreateEntity()
    {
        return new User(
            Guid.NewGuid(),
            Username,
            Password
        );
    }
};