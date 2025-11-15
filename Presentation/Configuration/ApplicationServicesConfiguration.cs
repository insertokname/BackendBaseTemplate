using Application.Commands.GenericCommands;
using Application.Commands.Users;
using Application.Commands.Users.CreateUser;
using Application.Commands.Users.LoginUser;
using Application.Query.GenericQueries;
using Application.Query.Users;

using Infrastructure;
using Infrastructure.Token;

namespace Presentation.Configuration
{
    public static class ApplicationServicesConfiguration
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            // Generic handlers
            services.AddScoped(typeof(DeleteByIdHandler<>));
            services.AddScoped(typeof(GetAllHandler<>));
            services.AddScoped(typeof(GetByIdHandler<>));

            // User handlers
            services.AddScoped(typeof(CreatePasswordUserHandler));
            services.AddScoped(typeof(CreateGoogleUserHandler));
            services.AddScoped(typeof(SetAdminHandler));
            services.AddScoped(typeof(LoginPasswordUserHandler));
            services.AddScoped(typeof(GetSelfHandler));

            // Services
            services.AddSingleton(typeof(TokenGeneratorService));
        }
    }
}
