using CRM.Models.DTO;
using CRM.Models.Enums;

namespace CRM.Core.Contracts;

/// <summary>
/// interface responsible to manipulate the data of Person
/// </summary>
public interface IPersonsService
{
    /// <summary>
    /// method responsible to add the person details to data store
    /// </summary>
    /// <param name="personAddRequest">input type is as PersonAddRequest </param>
    /// <returns>returns the response as object of PersonAddRequest</returns>
    PersonResponse AddPerson(PersonAddRequest? personAddRequest);

    /// <summary>
    /// method responsible to get all the Persons
    /// </summary>
    /// <returns>returns the resposne as list of PersonResponse object </returns>
    List<PersonResponse> GetPersons();

    /// <summary>
    /// method responsible to get the matched person
    /// </summary>
    /// <param name="PersonId">input type is as GUID</param>
    /// <returns>returns the resposne as object of PersonResponse </returns>
    PersonResponse? GetPersonByPersonId(Guid? PersonId);

    /// <summary>
    /// method responsible to filter the persons as per the input
    /// </summary>
    /// <param name="searchBy">input type is as string</param>
    /// <param name="searchString">input type is as string</param>
    /// <returns>returns the response as list of PersonResponse </returns>
    List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString);

    /// <summary>
    /// method responsible to sort the persons as per the input
    /// </summary>
    /// <param name="allPersons">input type is as list of PersonResponse </param>
    /// <param name="sortBy">input type is as string</param>
    /// <param name="sortOrderOptions">input type is as SortOrderEnum </param>
    /// <returns>returns the resposne as list of PersonResponse </returns>
    List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortOrderOptions sortOrderOptions );

    /// <summary>
    /// method responsible to update the person
    /// </summary>
    /// <param name="personUpdateRequest">input type is as PersonUpdateRequest </param>
    /// <returns>returns the response as object of PersonResponse </returns>
    PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    /// <summary>
    /// method responsible to delete the person
    /// </summary>
    /// <param name="personId">input type is as GUID</param>
    /// <returns>return true if the person is deleted </returns>
    bool DeletePerson(Guid? personId);
}
