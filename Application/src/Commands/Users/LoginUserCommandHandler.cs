using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.application.Commands.Users;

public class LoginUserCommandHandler
{
    private readonly IRepository<User> _userRepository;
    private readonly TokenProvider _tokenProvider;
    private readonly SecretsManager _secretsManager;

    public LoginUserCommandHandler(
        IRepository<User> entityRepository,
        TokenProvider tokenProvider,
        SecretsManager secretsManager
    )
    {
        _userRepository = entityRepository;
        _tokenProvider = tokenProvider;
        _secretsManager = secretsManager;
    }

    public async Task<LoginUserResult> HandleAsync(LoginUserCommand command)
    {
        User? user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u =>
            u.Username.Equals(command.Username)
        );

        if (command.Username == _secretsManager.DefaultAdminUsername &&
            command.Password == _secretsManager.DefaultAdminPassword)
        {
            return new LoginUserResult.Ok(
                _tokenProvider.Create(
                    new User(
                        Guid.Empty,
                        command.Username,
                        command.Password
                    )
                )
            );
        }

        if (user == null)
        {
            return new LoginUserResult.NoUsernameFound();
        }

        if (BCrypt.Net.BCrypt.EnhancedVerify(command.Password, user.Password))
        {
            return new LoginUserResult.Ok(_tokenProvider.Create(user));
        }
        else
        {
            return new LoginUserResult.IncorectPassword();
        }
    }
}