namespace BackendOlimpiadaIsto.application.Commands.Users;

public abstract class LoginUserResult
{
    private LoginUserResult() { }

    public sealed class Ok : LoginUserResult
    {
        public string Token { get; }
        public Ok(string token) => Token = token;
    }

    public sealed class NoUsernameFound : LoginUserResult { }
    public sealed class IncorectPassword : LoginUserResult { }
}
