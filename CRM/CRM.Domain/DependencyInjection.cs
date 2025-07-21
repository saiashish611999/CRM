using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Domain;
public static class DependencyInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CRMDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("database"));
        });
        return services;
    }
}
