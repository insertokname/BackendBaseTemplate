namespace BackendOlimpiadaIsto.application.Exceptions;

public class InvalidCredentialsException : Exception
{

    public InvalidCredentialsException()
        : base($"The username or password is incorect!")
    {
    }
}
