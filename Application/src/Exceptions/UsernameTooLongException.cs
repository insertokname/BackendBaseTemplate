namespace BackendBaseTemplate.application.Exceptions;

public class UsernameTooLongException : Exception
{
    public string Username { get; }

    public UsernameTooLongException(string username)
        : base($"The username '{username}' is too long!")
    {
        Username = username;
    }
}
