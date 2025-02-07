using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.application.Commands.Users;

public abstract class CreateUserResult
{
    private CreateUserResult() { }

    public sealed class Ok : CreateUserResult
    {
        public User User { get; }
        public Ok(User user) => User = user;
    }

    public sealed class UsernameTaken : CreateUserResult { }
}
