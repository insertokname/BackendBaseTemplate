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

    public LoginUserCommandHandler(
        IRepository<User> entityRepository,
        TokenProvider tokenProvider
    )
    {
        _userRepository = entityRepository;
        _tokenProvider = tokenProvider;
    }

    public async Task<LoginUserResult> HandleAsync(LoginUserCommand command)
    {
        User? user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u =>
            u.Username.Equals(command.Username)
        );

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