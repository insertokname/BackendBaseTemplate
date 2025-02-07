namespace BackendOlimpiadaIsto.domain.Entities;

public class User : Entity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    private User() : base(Guid.Empty) { }

    public User(Guid id, string username, string password) : base(id)
    {
        Username = username;
        Password = password;
    }
}