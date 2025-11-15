using Application.Exceptions;

using Domain;
using Domain.Entities;

using Infrastructure.Repositories;

namespace Application.Commands.Users.CreateUser
{
    public partial class CreatePasswordUserHandler(IRepository<User> userRepository)
    {

        public async Task<User> HandleAsync(CreatePasswordUserCommand command)
        {
            string sanitizedEmail = command.Email.Trim();
            if (sanitizedEmail.Length > Constants.MaxEmailLenght)
            {
                throw new EmailTooLongException(command.Email);
            }
            // TODO: check email for confirm

            if (userRepository.GetQueryable().Any(u => u.Email == sanitizedEmail))
            {
                throw new EmailTakenException(sanitizedEmail);
            }

            User newUser = new User(
                Guid.NewGuid(),
                sanitizedEmail,
                BCrypt.Net.BCrypt.EnhancedHashPassword(command.Password, Constants.WorkFactor),
                User.LoginType.Password
            );

            await userRepository.AddAsync(newUser);
            await userRepository.SaveChangesAsync();
            return newUser;
        }
    }
}