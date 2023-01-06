using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace RedTechChallenge.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddServiceContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<DataContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("AzureConnection")));
        }
    }
}
