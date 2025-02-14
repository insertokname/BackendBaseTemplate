namespace BackendOlimpiadaIsto.application.Query.Users;

public class GetUserByIdCommand
{
    public required Guid UserId { get; set; }
}