using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.application.Commands.Users;

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