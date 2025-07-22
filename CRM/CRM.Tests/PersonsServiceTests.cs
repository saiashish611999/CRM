using CRM.Core.Concretes;
using CRM.Core.Contracts;
using CRM.Models.DTO;
using CRM.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace CRM.Tests;
public class PersonsServiceTests
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    public PersonsServiceTests()
    {
        _countriesService = new CountriesService(new CRMDbContext(new DbContextOptionsBuilder<CRMDbContext>().Options) );
        _personsService = new PersonsService(_countriesService, new CRMDbContext(new DbContextOptionsBuilder<CRMDbContext>().Options));
    }

    #region AddPersonTests
    // if null PersonAddRequest, throw argument null exception
    [Fact]
    public async Task AddPerson_NullPersonAddRequest()
    {
        // arrange
        PersonAddRequest? request = null;

        // assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _personsService.AddPerson(request);
        });

    }
    // if null PersonName, throw ArgumentException
    [Fact]
    public async Task AddPerson_NullPersonName()
    {
        // arrange
        PersonAddRequest request = new PersonAddRequest() { PersonName = null };

        // assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _personsService.AddPerson(request);
        });
    }
    // if proper details, return the response as object of PersonResponse
    [Fact]
    public async Task AddPerson_ProperDetails()
    {
        // arrange
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest request = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("01-06-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonResponse response = await _personsService.AddPerson(request);

        Assert.True(response.PersonId != Guid.Empty);
    }
    #endregion

    #region GetPersonsTests
    // if null persons, return the resposne as empty
    [Fact]
    public async Task GetPersons_EmptyPersons()
    {
        // act
        List<PersonResponse> persons = await _personsService.GetPersons();
        // assert
        Assert.Empty(persons);
    }
    // if the person details exists, return the resposne as list of PersonResponse
    [Fact]
    public async Task GetPersons_PersonsExists()
    {
        // arrange
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest firstPersonRequest = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("06-01-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonAddRequest secondPersonRequest = new PersonAddRequest()
        {
            PersonName = "Praveen",
            Email = "praveen@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("23-01-1997"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };

        PersonResponse firstPersonResposne = await _personsService.AddPerson(firstPersonRequest);
        PersonResponse secondPersonResposne = await _personsService.AddPerson(secondPersonRequest);

        List<PersonResponse> persons = await _personsService.GetPersons();

        Assert.Contains(firstPersonResposne, persons);
        Assert.Contains(secondPersonResposne, persons);
    }
    #endregion

    #region GetPersonByPersonIdTests
    // if null personId, return null 
    [Fact]
    public async Task GetPersonByPersonId_NullPersonId()
    {
        // 
        Guid? personId = null;

        PersonResponse? person = await _personsService.GetPersonByPersonId(personId);
        Assert.Null(person);
    }
    // if person exists, return the resposne as object of PersonResponse
    [Fact]
    public async Task GetPersonByPersonId_PersonExists()
    {
        // arrange
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest request = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("01-06-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonResponse response = await _personsService.AddPerson(request);
        PersonResponse? person = await _personsService.GetPersonByPersonId(response.PersonId);
        Assert.Equal(response, person);
    }
    #endregion

    #region GetFilteredPersonsTests
    // if empty searchString, return all persons
    [Fact]
    public async Task GetFilteredPersons_EmptySearchString()
    {
        // arrange
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest firstPersonRequest = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("06-01-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonAddRequest secondPersonRequest = new PersonAddRequest()
        {
            PersonName = "Praveen",
            Email = "praveen@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("23-01-1997"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };

        PersonResponse firstPersonResposne = await _personsService.AddPerson(firstPersonRequest);
        PersonResponse secondPersonResposne = await _personsService.AddPerson(secondPersonRequest);

        List<PersonResponse> persons = await _personsService.GetPersons();
        List<PersonResponse> filteredPersons = await _personsService.GetFilteredPersons(nameof(PersonResponse.PersonName), "");

        persons.ForEach(person => Assert.Contains(person, filteredPersons));
    }
    // if searchString, return matching persons
    [Fact]
    public async Task GetFilteredPersons_SearchString()
    {
        // arrange
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest firstPersonRequest = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("06-01-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonAddRequest secondPersonRequest = new PersonAddRequest()
        {
            PersonName = "Praveen",
            Email = "praveen@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("23-01-1997"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };

        PersonResponse firstPersonResposne = await _personsService.AddPerson(firstPersonRequest);
        PersonResponse secondPersonResposne = await _personsService.AddPerson(secondPersonRequest);

        List<PersonResponse> persons = await _personsService.GetPersons();
        persons = persons.Where(person => person.PersonName!.Contains("pr", StringComparison.OrdinalIgnoreCase)).ToList();
        List<PersonResponse> filteredPersons = await _personsService.GetFilteredPersons(nameof(PersonResponse.PersonName), "pr");

        persons.ForEach(person => Assert.Contains(person, filteredPersons));
    }
    #endregion

    #region GetSortedPersonsTests
    // if sortOrder is descensding, return the response os descending
    [Fact]
    public async Task GetSortedPersons_Descending()
    {
        // arrange
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest firstPersonRequest = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("06-01-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonAddRequest secondPersonRequest = new PersonAddRequest()
        {
            PersonName = "Praveen",
            Email = "praveen@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("23-01-1997"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };

        PersonResponse firstPersonResposne = await _personsService.AddPerson(firstPersonRequest);
        PersonResponse secondPersonResposne = await _personsService.AddPerson(secondPersonRequest);

        List<PersonResponse> persons = await _personsService.GetPersons();
        List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, nameof(PersonResponse.PersonName), Models.Enums.SortOrderOptions.desc);
        persons = persons.OrderByDescending(person => person.PersonName).ToList();
        for (int index = 0; index < sortedPersons.Count(); index++)
        {
            Assert.Equal(persons[index], sortedPersons[index]);
        }
    }
    #endregion

    #region UpdatePersonTests
    // if null personUpdateRequest, throw new ArgumentNullException
    [Fact]
    public async Task UpdatePerson_NullPersonUpdateRequest()
    {
        PersonUpdateRequest? request = null;
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _personsService.UpdatePerson(request);
        });
    }
    // if null PersonName, throw new ArgumentException
    [Fact]
    public async Task UpdatePerson_NullPersonName()
    {
        PersonUpdateRequest request = new PersonUpdateRequest() { PersonName = null };
         await Assert.ThrowsAsync<ArgumentException>(async() =>
         {
             await _personsService.UpdatePerson(request);
         });
    }
    // if proper details, return response as object of PersonResponse
    [Fact]
    public async Task UpdatePerson_ProperDetails()
    {
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest request = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("01-06-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonResponse response = await _personsService.AddPerson(request);
        PersonUpdateRequest updateRequest = response.FromPersonResponseToPersonUpdateRequest();
        PersonResponse updateResposne = await _personsService.UpdatePerson(updateRequest);
        PersonResponse? personResponse = await _personsService.GetPersonByPersonId(updateResposne.PersonId);

        Assert.Equal(updateResposne, personResponse);
    }
    #endregion

    #region DeletePersonTests
    // if person doesn't exisx, return false
    [Fact]
    public async Task DeletePerson_NoPerson()
    {
        Guid personId = Guid.NewGuid();
        bool deleted = await _personsService.DeletePerson(personId);
        Assert.False(deleted);
    }
    // if person exists, return true
    [Fact]
    public async Task DeletePerson_PersonExists()
    {
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "India" };
        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest request = new PersonAddRequest()
        {
            PersonName = "sai ashish",
            Email = "ashish@gmail.com",
            Gender = Models.Enums.GenderOptions.Male,
            DateOfBirth = Convert.ToDateTime("01-06-1999"),
            ReceiveNewsLetters = true,
            Address = "something",
            CountryId = countryResponse.CountryId
        };
        PersonResponse response = await _personsService.AddPerson(request);
        bool deleted = await _personsService.DeletePerson(response.PersonId);
        Assert.True(deleted);
    }
    #endregion
}
