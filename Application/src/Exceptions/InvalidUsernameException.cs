namespace BackendOlimpiadaIsto.application.Exceptions;

public class InvalidUsernameCharacterException : Exception
{
    public string Username { get; }

    public InvalidUsernameCharacterException(string username)
        : base($"The username '{username}' contains a characters that are invalid!")
    {
        Username = username;
    }
}
