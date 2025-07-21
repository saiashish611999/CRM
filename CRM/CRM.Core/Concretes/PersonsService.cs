using CRM.Core.Contracts;
using CRM.Core.Helpers;
using CRM.Domain;
using CRM.Domain.Entities;
using CRM.Models.DTO;
using CRM.Models.Enums;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Concretes;
public class PersonsService: IPersonsService
{
    private readonly CRMDbContext _database;
    private readonly ICountriesService _coutnriesService;
    public PersonsService(ICountriesService countriesService, CRMDbContext database)
    {
        _coutnriesService = countriesService;
        _database = database;      

    }

    #region PrivateMethod
    //private PersonResponse FromPersonToPersonResponse(Person person)
    //{
    //    PersonResponse response = person.FromPersonToPersonResponse();
    //    // response.Country = _coutnriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
    //    response.Country = person.Country?.CountryName; // due to navigation property
    //    return response;
    //}
    #endregion

    #region AddPerson
    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest is null)
            throw new ArgumentNullException(nameof(personAddRequest));
        ValidationHelper.ObjectValidator(personAddRequest);
        Person person = personAddRequest.FromPersonAddRequestToPerson();
        person.PersonId = Guid.NewGuid();

        //// using Stored Procedure
        //_database.sp_InsertPerson(person);

        _database.Persons.Add(person);
        _database.SaveChanges();

        PersonResponse resposne = person.FromPersonToPersonResponse();
        return resposne;
    }
    #endregion

    #region GetPersons
    public List<PersonResponse> GetPersons()
    {
        //List<Person> persons = _database.sp_GetAllPersons(); // eager loading
        //return persons.Select(FromPersonToPersonResponse).ToList();
        //// instance methods can not be performed on sql object. load them eagerly into the in memory and then perform the action.
       
        
        // using LINQ method
        List<Person> persons = _database.Persons.Include("Country").ToList();
        return persons.Select(person => person.FromPersonToPersonResponse()).ToList();
    }
    #endregion

    #region GetPersonByPersonId
    public PersonResponse? GetPersonByPersonId(Guid? personId)
    {
        Person? matchingPerson = _database.Persons.Include("Country").FirstOrDefault(person => person.PersonId == personId);
        if (matchingPerson is null)
            return null;
        return matchingPerson.FromPersonToPersonResponse();
    }
    #endregion

    #region GetFilteredPersons
    public List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString)
    {
        List<PersonResponse> filteredPersons = GetPersons();
        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return filteredPersons;
        var prop = typeof(PersonResponse).GetProperty(searchBy);
        if (prop is null)
            return filteredPersons;
        filteredPersons = filteredPersons.Where(person =>
        {
            var value = prop.GetValue(person);
            if (value is null)
                return true;
            if (searchBy == nameof(PersonResponse.Gender))
                return value.ToString()!.Equals(searchString, StringComparison.OrdinalIgnoreCase);
            return value is DateTime other ?
                   other.ToString("dd MM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) :
                   value.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase);

        }).ToList();
        return filteredPersons;
    }
    #endregion

    #region GetSortedPersons
    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortOrderOptions sortOrderOptions)
    {
        List<PersonResponse> sortedPersons = allPersons;

        if (string.IsNullOrEmpty(sortBy))
            return sortedPersons;

        var prop = typeof(PersonResponse).GetProperty(sortBy);
        if(prop is null)
            return sortedPersons;

        switch (sortOrderOptions)
        {
            case SortOrderOptions.asc:
                sortedPersons = sortedPersons.OrderBy(person => prop.GetValue(person)).ToList();
                break;
            case SortOrderOptions.desc:
                sortedPersons = sortedPersons.OrderByDescending(person => prop.GetValue(person)).ToList();
                break;
        }

        return sortedPersons;
    }
    #endregion

    #region UpdatePerson
    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest is null)
            throw new ArgumentNullException(nameof(personUpdateRequest));
        ValidationHelper.ObjectValidator(personUpdateRequest);
        Person? person = _database.Persons.FirstOrDefault(person => person.PersonId == personUpdateRequest.PersonId);
        if (person is null)
            throw new ArgumentException("Invalid person id");
        person.PersonName = personUpdateRequest.PersonName;
        person.Email = personUpdateRequest.Email;
        person.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
        person.DateOfBirth = personUpdateRequest.DateOfBirth;
        person.Address = personUpdateRequest.Address;
        person.Gender = personUpdateRequest.Gender.ToString();
        person.CountryId = personUpdateRequest.CountryId;
        _database.SaveChanges();

        PersonResponse resposne = person.FromPersonToPersonResponse();
        return resposne;
    }
    #endregion

    #region DeletePerson
    public bool DeletePerson(Guid? personId)
    {
        if (personId is null)
            throw new ArgumentException("null personID");
        Person? person = _database.Persons.FirstOrDefault(person => person.PersonId == personId);
        if (person is null) return false;
        _database.Persons.Remove(person);
        _database.SaveChanges();

        return true;
    }
    #endregion
}
