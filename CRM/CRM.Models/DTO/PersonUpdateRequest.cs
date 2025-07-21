using CRM.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTO;
public class PersonUpdateRequest
{
    public Guid PersonId { get; set; }
    [Required(ErrorMessage = "PersonName is a required field")]
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid CountryId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public GenderOptions Gender { get; set; }
}
