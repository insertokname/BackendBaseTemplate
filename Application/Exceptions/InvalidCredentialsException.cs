namespace BackendBaseTemplate.application.Exceptions;

public class BadCredentialsException : Exception
{

    public BadCredentialsException()
        : base($"The username or password is incorect!")
    {
    }
}
