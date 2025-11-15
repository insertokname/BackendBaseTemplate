using Domain.Entities;

namespace Application.Exceptions
{
    public class BadLoginMethodException(
        User.LoginType expectedLoginType,
        User.LoginType recievedLoginType) : Exception($"Bad login method! Expected {expectedLoginType}, instead got {recievedLoginType}")
    {
        public User.LoginType ExpectedLoginType { get; } = expectedLoginType;
        public User.LoginType RecievedLoginType { get; } = recievedLoginType;
    }

}