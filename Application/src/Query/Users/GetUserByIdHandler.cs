using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using domain.ValueObjects;

namespace BackendOlimpiadaIsto.application.Query.Users;


public class GetUserByIdHandler
{
    private readonly IRepository<User> _userRepository;

    public GetUserByIdHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> HandleAsync(GetUserByIdQuery query)
    {
        User? user = await _userRepository.GetByIdAsync(query.UserId);
        if (user == null)
            throw new ArgumentException($"No user found by id {query.UserId}");
        return user;
    }
}