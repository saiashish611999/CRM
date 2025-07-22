using CRM.Core.Concretes;
using CRM.Core.Contracts;
using CRM.Models.DTO;
using CRM.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CRM.Tests;
public class CountriesServiceTests
{
    private readonly ICountriesService _countriesService;
    public CountriesServiceTests()
    {
        _countriesService = new CountriesService(new CRMDbContext(new DbContextOptionsBuilder<CRMDbContext>().Options));
    }

    #region AddRegionTests
    // if null countryAddRequest, throw new ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountryAddRequest()
    {
        // arrange
        CountryAddRequest? request = null;

        // assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _countriesService.AddCountry(request);
        });
    }
    // if null countryName, throw new ArgumentException
    [Fact]
    public async Task AddCountry_CountryName()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest() { CountryName = null };

        // assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _countriesService.AddCountry(request);
        });
    }
    // if duplicate country name, throw new ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountry()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest() { CountryName = "India" };

        // act
        await _countriesService.AddCountry(request);

        // assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _countriesService.AddCountry(request);
        });
    }
    // if proper details, return the response as object of CountryResponse
    [Fact]
    public async Task AddCountry_ProperCountryDetails()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = "India"
        };

        // act
        CountryResponse response = await _countriesService.AddCountry(request);

        // assert
        Assert.True(response.CountryId != Guid.Empty);
    }
    #endregion

    #region GetAllCountriesTests
    // if no country objects, return empty
    [Fact]
    public async Task GetAllCountries_NoCountries()
    {
        // act
        List<CountryResponse> countries = await _countriesService.GetAllCountries();

        // assert
        Assert.Empty(countries);
    }
    // if country details exists, return the response as list of CountryResponse
    [Fact]
    public async Task GetAllCountries_CountriesExists()
    {
        // arrange
        CountryAddRequest firstRequest = new CountryAddRequest() { CountryName = "India" };
        CountryAddRequest secondRequest = new CountryAddRequest() { CountryName = "Japan" };
        CountryResponse firstResponse = await _countriesService.AddCountry(firstRequest);
        CountryResponse secondResponse = await _countriesService.AddCountry(secondRequest);

        // act
        List<CountryResponse> countries = await _countriesService.GetAllCountries();

        // arrange
        Assert.Contains(firstResponse, countries);
        Assert.Contains(secondResponse, countries);
    }
    #endregion

    #region GetCountryByCountryIdTests
    // if null countryId, return null
    [Fact]
    public async Task GetCountryByCountryId_NullCountryId()
    {
        // arrange
        Guid? countryId = null;

        // act
        CountryResponse? response = await _countriesService.GetCountryByCountryId(countryId);

        // assert
        Assert.Null(response);
    }
    // if country exists, return the object as CountryResponse
    [Fact]
    public async Task GetCountryByCountryId_CountryExists()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = "India"
        };
        CountryResponse response = await _countriesService.AddCountry(request);

        // act
        CountryResponse? countryResponse = await _countriesService.GetCountryByCountryId(response.CountryId);
        List<CountryResponse> countries = await _countriesService.GetAllCountries();

        // assert
        Assert.Equal(countryResponse, response);
        Assert.Contains(countryResponse, countries);
    }
    #endregion
}
