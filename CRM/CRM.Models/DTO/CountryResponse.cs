using CRM.Domain.Entities;

namespace CRM.Models.DTO;
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CountryResponse other &&
               CountryId == other.CountryId &&
               CountryName == other.CountryName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CountryId, CountryName);
    }
}

public static class CountryExtensions
{
    public static CountryResponse FromCountryToCountryResponse(this Country country)
    {
        return new CountryResponse()
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName,
        };
    }
}