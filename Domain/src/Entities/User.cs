using domain.ValueObjects;

namespace BackendOlimpiadaIsto.domain.Entities;

public class User : Entity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<AnsweredQuestion> AnsweredQuestions { get; set; } = new List<AnsweredQuestion>();
    public bool IsAdmin { get; set; } = false;

    protected User() : base(Guid.Empty) { }

    public User(Guid id, string username, string password, bool isAdmin = false) : base(id)
    {
        Username = username;
        Password = password;
        IsAdmin = isAdmin;
    }
}