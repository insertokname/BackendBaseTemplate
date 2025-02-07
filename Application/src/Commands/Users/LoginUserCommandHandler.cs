using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Repositories;
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

    public async Task<string?> HandleAsync(LoginUserCommand command)
    {
        // E newEntity = command.CreateEntity();
        // await _userRepository.AddAsync(newEntity);
        // await _userRepository.SaveChangesAsync();
        // return newEntity;
        // var user = new User(Guid.Parse("5524fd51-c43a-4ecd-bd4a-76f61041789b"));

        User? user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u =>
            u.Username.Equals(command.Username) &&
            u.Password.Equals(command.Password)
        );

        if (user == null)
        {
            return null;
        }

        return _tokenProvider.Create(user);
    }
}