namespace Application.Commands.Users.LoginUser
{
    public class LoginPasswordUserCommand
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    };
}