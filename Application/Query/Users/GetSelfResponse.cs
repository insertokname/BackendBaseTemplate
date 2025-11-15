namespace Application.Query.Users
{
    public class GetSelfResponse
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
    };
}