namespace Application.Exceptions
{
    public class EmailTooLongException(string email) : Exception($"The email '{email}' is too long!")
    {
        public string Email { get; } = email;
    }
}