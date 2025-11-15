namespace Application.Commands.Users.CreateUser
{
    public class CreateGoogleUserCommand
    {
        public required string Email { get; set; }
        public required string GoogleId { get; set; }
    }
}