using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain.Entities;
public class Person
{
    [Key]
    public Guid PersonId { get; set; }

    [StringLength(40)]
    public string? PersonName { get; set; }

    [StringLength(25)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Address { get; set; }
    public Guid CountryId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public string? Gender { get; set; }
    public string? TIN { get; set; }

    [ForeignKey("CountryId")]
    public Country? Country { get; set; }
}

