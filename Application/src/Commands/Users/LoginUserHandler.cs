using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.application.Commands.Users;

public class LoginUserHandler
{
    private readonly IRepository<User> _userRepository;
    private readonly TokenProvider _tokenProvider;
    private readonly SecretsManager _secretsManager;

    public LoginUserHandler(
        IRepository<User> entityRepository,
        TokenProvider tokenProvider,
        SecretsManager secretsManager
    )
    {
        _userRepository = entityRepository;
        _tokenProvider = tokenProvider;
        _secretsManager = secretsManager;
    }

    public async Task<string> HandleAsync(LoginUserCommand command)
    {
        User? user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u =>
            u.Username.Equals(command.Username)
        );

        if (command.Username == _secretsManager.DefaultAdminUsername &&
            command.Password == _secretsManager.DefaultAdminPassword)
        {
            return _tokenProvider.Create(
                    new User(
                        _secretsManager.DefaultAdminGuid,
                        command.Username,
                        command.Password,
                        isAdmin: true
                    )
                );
        }

        if (user == null)
            throw new BadCredentialsException();


        if (BCrypt.Net.BCrypt.EnhancedVerify(command.Password, user.Password))
            return _tokenProvider.Create(user);
        else
            throw new BadCredentialsException();

    }
}