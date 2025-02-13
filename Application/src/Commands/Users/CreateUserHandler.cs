using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Repositories;

namespace BackendOlimpiadaIsto.application.Commands.Users;


public class CreateUserHandler
{
    private readonly IRepository<User> _userRepository;

    public CreateUserHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateUserResult> HandleAsync(CreateUserCommand command)
    {
        if (_userRepository.GetQueryable().Any(u => u.Username == command.Username))
        {
            return new CreateUserResult.UsernameTaken();
        }


        User newUser = new User(
            Guid.NewGuid(),
            command.Username,
            BCrypt.Net.BCrypt.EnhancedHashPassword(command.Password, Constants.WorkFactor)
        );

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();
        return new CreateUserResult.Ok(newUser);
    }
}