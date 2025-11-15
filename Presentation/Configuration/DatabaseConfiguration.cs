using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Presentation.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
