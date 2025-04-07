using BackendBaseTemplate.domain.Commands.DataTransferObjects;
using domain.ValueObjects;

namespace BackendBaseTemplate.domain.Entities;

public class User : Entity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; } = false;
    protected User() : base(Guid.Empty) { }
    public User(
        Guid id,
        string username,
        string password,
        bool isAdmin = false
    ) : base(id)
    {
        Username = username;
        Password = password;
        IsAdmin = isAdmin;
    }
}