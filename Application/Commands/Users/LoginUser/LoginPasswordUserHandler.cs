using Application.Exceptions;

using Domain.Entities;

using Infrastructure.Repositories;
using Infrastructure.Token;

using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.LoginUser
{
    public class LoginPasswordUserHandler(
        IRepository<User> userRepository,
        TokenGeneratorService tokenProvider
        )
    {
        public async Task<string> HandleAsync(LoginPasswordUserCommand command)
        {
            User? user = await userRepository.GetQueryable().FirstOrDefaultAsync(u =>
                u.Email.Equals(command.Email)
            );

            if (user == null)
            {
                throw new BadCredentialsException();
            }

            if (user.UserLoginType != User.LoginType.Password)
            {
                throw new BadLoginMethodException(User.LoginType.Password, user.UserLoginType);
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(command.Password, user.Secret))
            {
                return tokenProvider.Create(user);
            }
            else
            {
                throw new BadCredentialsException();
            }
        }
    }
}