using Application.Query.GenericQueries;

using Domain.Entities;

using Infrastructure.Repositories;

namespace Application.Commands.Users
{
    public class SetAdminHandler(
        IRepository<User> userRepository,
        GetByIdHandler<User> getByIdHandler
        )
    {
        public async Task HandleAsync(SetAdminCommand command)
        {
            var user = await getByIdHandler.HandleAsync(new GetByIdQuery() { EntityId = command.UserId });
            user.IsAdmin = true;
            await userRepository.SaveChangesAsync();
        }
    }
}