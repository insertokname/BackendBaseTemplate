namespace Application.Exceptions
{
    public class EmailTakenException(string email) : Exception($"The email '{email}' is already taken.")
    {
        public string Email { get; } = email;
    }
}
