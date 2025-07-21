using CRM.Domain.Entities;
using CRM.Models.Enums;
using System.Runtime.CompilerServices;

namespace CRM.Models.DTO;
public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid CountryId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public string? Gender { get; set; }
    public double? Age { get; set; }
    public string? Country { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PersonResponse other &&
               PersonId == other.PersonId;
    }

    public override int GetHashCode()
    {
        return PersonId.GetHashCode();
    }

    public PersonUpdateRequest FromPersonResponseToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonId = PersonId,
            PersonName = PersonName,
            Email = Email,
            Address = Address,
            CountryId = CountryId,
            DateOfBirth = DateOfBirth,
            ReceiveNewsLetters = ReceiveNewsLetters,
            Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender!)
        };
    }
}

public static class PersonExtensions
{
    //private PersonResponse FromPersonToPersonResponse(Person person)
    //{
    //    PersonResponse response = person.FromPersonToPersonResponse();
    //    // response.Country = _coutnriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
    //    response.Country = person.Country?.CountryName; // due to navigation property
    //    return response;
    //}
    public static PersonResponse FromPersonToPersonResponse(this Person person)
    {
        return new PersonResponse()
        {
            PersonId = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            Address = person.Address,
            CountryId = person.CountryId,
            DateOfBirth = person.DateOfBirth,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Gender = person.Gender,
            Age = (person.DateOfBirth is not null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
            Country = person.Country?.CountryName,

        };
    }
}
