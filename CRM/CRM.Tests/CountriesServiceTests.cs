using CRM.Core.Concretes;
using CRM.Core.Contracts;
using CRM.Models.DTO;
using CRM.Domain;
using Microsoft.EntityFrameworkCore;

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
    public void AddCountry_NullCountryAddRequest()
    {
        // arrange
        CountryAddRequest? request = null;

        // assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }
    // if null countryName, throw new ArgumentException
    [Fact]
    public void AddCountry_CountryName()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest() { CountryName = null };

        // assert
        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }
    // if duplicate country name, throw new ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountry()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest() { CountryName = "India" };

        // act
        _countriesService.AddCountry(request);

        // assert
        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }
    // if proper details, return the response as object of CountryResponse
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = "India"
        };

        // act
        CountryResponse response = _countriesService.AddCountry(request);

        // assert
        Assert.True(response.CountryId != Guid.Empty);
    }
    #endregion

    #region GetAllCountriesTests
    // if no country objects, return empty
    [Fact]
    public void GetAllCountries_NoCountries()
    {
        // act
        List<CountryResponse> countries = _countriesService.GetAllCountries();

        // assert
        Assert.Empty(countries);
    }
    // if country details exists, return the response as list of CountryResponse
    [Fact]
    public void GetAllCountries_CountriesExists()
    {
        // arrange
        CountryAddRequest firstRequest = new CountryAddRequest() { CountryName = "India" };
        CountryAddRequest secondRequest = new CountryAddRequest() { CountryName = "Japan" };
        CountryResponse firstResponse = _countriesService.AddCountry(firstRequest);
        CountryResponse secondResponse =_countriesService.AddCountry(secondRequest);

        // act
        List<CountryResponse> countries = _countriesService.GetAllCountries();

        // arrange
        Assert.Contains(firstResponse, countries);
        Assert.Contains(secondResponse, countries);
    }
    #endregion

    #region GetCountryByCountryIdTests
    // if null countryId, return null
    [Fact]
    public void GetCountryByCountryId_NullCountryId()
    {
        // arrange
        Guid? countryId = null;

        // act
        CountryResponse? response = _countriesService.GetCountryByCountryId(countryId);

        // assert
        Assert.Null(response);
    }
    // if country exists, return the object as CountryResponse
    [Fact]
    public void GetCountryByCountryId_CountryExists()
    {
        // arrange
        CountryAddRequest request = new CountryAddRequest()
        {
            CountryName = "India"
        };
        CountryResponse response = _countriesService.AddCountry(request);

        // act
        CountryResponse? countryResponse = _countriesService.GetCountryByCountryId(response.CountryId);
        List<CountryResponse> countries = _countriesService.GetAllCountries();

        // assert
        Assert.Equal(countryResponse, response);
        Assert.Contains(countryResponse, countries);
    }
    #endregion
}
