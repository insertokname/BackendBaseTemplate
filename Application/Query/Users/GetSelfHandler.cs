using Application.Query.GenericQueries;

using Domain.Entities;


namespace Application.Query.Users
{
    public class GetSelfHandler(
        GetByIdHandler<User> getByIdHandler
        )
    {
        public async Task<GetSelfResponse> HandleAsync(GetSelfQuery query)
        {
            var user = await getByIdHandler.HandleAsync(new GetByIdQuery() { EntityId = query.SelfId });
            return new GetSelfResponse()
            {
                Id = user.Id,
                Email = user.Email,
            };
        }
    }
}