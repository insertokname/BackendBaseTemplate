using Application.Exceptions;

using Domain.Entities;

using Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.CreateUser
{
    public partial class CreateGoogleUserHandler(IRepository<User> userRepository)
    {

        public async Task<User> HandleAsync(CreateGoogleUserCommand command)
        {
            User? user = await userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Secret == command.GoogleId || u.Email == command.Email);

            if (user == null)
            {
                user = new User(
                    Guid.NewGuid(),
                    command.Email,
                    command.GoogleId,
                    User.LoginType.Google
                );

                await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();
                return user;
            }
            else
            {
                if (user.UserLoginType != User.LoginType.Google)
                {
                    throw new BadLoginMethodException(User.LoginType.Google, user.UserLoginType);
                }

                bool changed = false;
                if (user.Email != command.Email)
                {
                    user.Email = command.Email;
                    changed = true;
                }
                if (string.IsNullOrEmpty(user.Secret) && user.Email == command.Email)
                {
                    user.Secret = command.GoogleId;
                    changed = true;
                }
                if (changed)
                {
                    userRepository.Update(user);
                    await userRepository.SaveChangesAsync();
                }
                return user;
            }
        }
    }
}