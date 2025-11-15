namespace Domain.Entities
{
    public class User : Entity
    {
        public string Email { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public LoginType UserLoginType { get; set; }
        protected User() : base(Guid.Empty) { }

        public User(
            Guid id,
            string email,
            string secret,
            LoginType userLoginType
        ) : base(id)
        {
            Email = email;
            Secret = secret;
            UserLoginType = userLoginType;
        }

        public enum LoginType
        {
            Google,
            Password
        }
    }
}
