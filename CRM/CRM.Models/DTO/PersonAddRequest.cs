using CRM.Domain.Entities;
using CRM.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTO;
public class PersonAddRequest
{
    [Required(ErrorMessage = "PersonName is a required field")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Email is a required field")]
    [EmailAddress(ErrorMessage = "Email is not in a correct format")]
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid CountryId { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
    public GenderOptions Gender { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    

    public Person FromPersonAddRequestToPerson()
    {
        return new Person()
        {
            PersonName = PersonName,
            Email = Email,
            Address = Address,
            CountryId = CountryId,
            DateOfBirth = DateOfBirth,
            ReceiveNewsLetters = ReceiveNewsLetters,
            Gender = Gender.ToString()
        };
    }
}
