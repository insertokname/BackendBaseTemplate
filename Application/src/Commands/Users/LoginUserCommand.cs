using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.application.Commands.Users;


public class LoginUserCommand
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