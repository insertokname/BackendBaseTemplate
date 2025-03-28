using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure;
using BackendBaseTemplate.infrastructure.Repositories;

namespace BackendBaseTemplate.application.Commands.Users;


public class SetUserAdminHandler
{
    private readonly IRepository<User> _userRepository;

    public SetUserAdminHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(SetUserAdminCommand command)
    {
        User? user = await _userRepository.GetByIdAsync(command.UserId);

        if (user == null)
            throw new ArgumentException($"No username found by the id: {command.UserId}");

        user.IsAdmin = command.IsAdmin;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }
}