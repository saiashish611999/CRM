using CRM.Core.Contracts;
using CRM.Domain;
using CRM.Domain.Entities;
using CRM.Models.DTO;

namespace CRM.Core.Concretes;
public class CountriesService: ICountriesService
{
    private CRMDbContext _database;
    public CountriesService( CRMDbContext database)
    {
        _database = database;
    }

    #region AddCountry
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        // validations
        if (countryAddRequest is null)
            throw new ArgumentNullException(nameof(countryAddRequest));
        if (countryAddRequest.CountryName is null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        if (_database.Countries.Count(country => country.CountryName == countryAddRequest.CountryName) > 0)
            throw new ArgumentException("Country already exists!");

        Country country = countryAddRequest.FromCountryAddRequestToCountry();
        country.CountryId = Guid.NewGuid();
        _database.Countries.Add(country);
        _database.SaveChanges();

        CountryResponse response = country.FromCountryToCountryResponse();
        return response;
    }
    #endregion

    #region GetAllCountries
    public List<CountryResponse> GetAllCountries()
    {
        return _database.Countries.Select(country => country.FromCountryToCountryResponse()).ToList();
    }
    #endregion

    #region GetCountryByCountryId
    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        return _database.Countries.FirstOrDefault(country => country.CountryId == countryId)?.FromCountryToCountryResponse();
    }
    #endregion
}
