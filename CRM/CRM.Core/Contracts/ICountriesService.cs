using CRM.Models.DTO;

namespace CRM.Core.Contracts;

/// <summary>
/// interface responsible to manipulate the Country entity
/// </summary>
public interface ICountriesService
{
    /// <summary>
    /// method responsible to add the country to the datastore
    /// </summary>
    /// <param name="countryAddRequest">input type is as CountryAddRequest</param>
    /// <returns>returns the response as object of CountryResponse </returns>
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

    /// <summary>
    /// method responsible to return all the countries present in database
    /// </summary>
    /// <returns>return the response as list of CountryResponse object </returns>
    List<CountryResponse> GetAllCountries();

    /// <summary>
    /// method responsible to return the matching Country object
    /// </summary>
    /// <param name="countryId">input type is as GUID</param>
    /// <returns>return the response as object of CountryResponse </returns>
    CountryResponse? GetCountryByCountryId(Guid? countryId);
}
