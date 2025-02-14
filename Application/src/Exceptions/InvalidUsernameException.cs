namespace BackendOlimpiadaIsto.application.Exceptions;

public class InvalidUsernameException : Exception
{
    public string Username { get; }

    public InvalidUsernameException(string username)
        : base($"The username '{username}' is invalid!")
    {
        Username = username;
    }
}
