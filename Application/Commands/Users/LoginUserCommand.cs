using BackendBaseTemplate.domain.Entities;

namespace BackendBaseTemplate.application.Commands.Users;


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