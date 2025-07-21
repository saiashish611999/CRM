using CRM.Core.Concretes;
using CRM.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICountriesService, CountriesService>();
        services.AddScoped<IPersonsService, PersonsService>();
        return services;
    }
}
