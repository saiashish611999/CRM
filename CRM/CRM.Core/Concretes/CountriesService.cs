using CRM.Core.Contracts;
using CRM.Domain;
using CRM.Domain.Entities;
using CRM.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Concretes;
public class CountriesService: ICountriesService
{
    private CRMDbContext _database;
    public CountriesService( CRMDbContext database)
    {
        _database = database;
    }

    #region AddCountry
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
        // validations
        if (countryAddRequest is null)
            throw new ArgumentNullException(nameof(countryAddRequest));
        if (countryAddRequest.CountryName is null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        if (await _database.Countries.CountAsync(country => country.CountryName == countryAddRequest.CountryName) > 0)
            throw new ArgumentException("Country already exists!");

        Country country = countryAddRequest.FromCountryAddRequestToCountry();
        country.CountryId = Guid.NewGuid();
        _database.Countries.Add(country);
        await  _database.SaveChangesAsync();

        CountryResponse response = country.FromCountryToCountryResponse();
        return response;
    }
    #endregion

    #region GetAllCountries
    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return await _database.Countries.Select(country => country.FromCountryToCountryResponse()).ToListAsync();
    }
    #endregion

    #region GetCountryByCountryId
    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        Country? country = await _database.Countries.FirstOrDefaultAsync(country => country.CountryId == countryId);
        return country is not null ? country.FromCountryToCountryResponse() : null;
    }
    #endregion
}
