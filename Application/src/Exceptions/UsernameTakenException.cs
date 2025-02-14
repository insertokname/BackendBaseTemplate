namespace BackendOlimpiadaIsto.application.Exceptions;

public class UsernameTakenException : Exception
{
    public string Username { get; }

    public UsernameTakenException(string username)
        : base($"The username '{username}' is taken!")
    {
        Username = username;
    }
}
