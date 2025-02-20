using System.Text.RegularExpressions;
using BackendOlimpiadaIsto.application.Exceptions;
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

    public async Task<User> HandleAsync(CreateUserCommand command)
    {
        string sanitizedUsername = command.Username.Trim();
        if (sanitizedUsername.Length < 3 || sanitizedUsername.Length > 30 ||
            !Regex.IsMatch(sanitizedUsername, "^[a-zA-Z0-9_ ăâîțșĂÂÎȚȘ]+$"))
        {
            throw new InvalidUsernameException(command.Username);
        }

        if (_userRepository.GetQueryable().Any(u => u.Username == sanitizedUsername))
        {
            throw new UsernameTakenException(sanitizedUsername);
        }

        User newUser = new User(
            Guid.NewGuid(),
            sanitizedUsername,
            BCrypt.Net.BCrypt.EnhancedHashPassword(command.Password, Constants.WorkFactor)
        );

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();
        return newUser;
    }
}