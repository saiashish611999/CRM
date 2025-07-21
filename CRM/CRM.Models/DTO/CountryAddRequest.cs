using CRM.Domain.Entities;

namespace CRM.Models.DTO;
public class CountryAddRequest
{
    public string? CountryName { get; set; }

    public Country FromCountryAddRequestToCountry()
    {
        return new Country()
        {
            CountryName = CountryName,
        };
    }
}
