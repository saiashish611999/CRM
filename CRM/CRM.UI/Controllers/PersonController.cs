using CRM.Core.Contracts;
using CRM.Models.DTO;
using CRM.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.UI.Controllers;

[Route("[controller]")]
public class PersonController : Controller
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    public PersonController(IPersonsService personsService, ICountriesService countriesService)
    {
        _personsService = personsService;
        _countriesService = countriesService;
    }
    [Route("[action]")]
    [Route("/")]
    [HttpGet]
    public IActionResult Index(string searchBy,
                               string searchString,
                               string sortBy,
                               SortOrderOptions sortOrder = SortOrderOptions.asc)
    {
        List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(PersonResponse.PersonName), "name"},
            { nameof(PersonResponse.Email), "Email"},
            { nameof(PersonResponse.DateOfBirth), "Date Of Birth"},
            { nameof(PersonResponse.Age), "Age"},
            { nameof(PersonResponse.Gender), "Gender"},
            { nameof(PersonResponse.Address), "Address"},
            { nameof(PersonResponse.Country), "Country"},
            { nameof(PersonResponse.ReceiveNewsLetters), "Receive News Letters"},
        };

        // searching
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        // sorting
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;
        List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);

        return View(sortedPersons);
    }

    #region Create Person
    [HttpGet]
    [Route("[action]")]
    public IActionResult Create()
    {
        List<CountryResponse> countries = _countriesService.GetAllCountries();
        ViewBag.Countries = countries.Select(country => new SelectListItem()
        {
            Text = country.CountryName,
            Value = country.CountryId.ToString()
        });
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Create(PersonAddRequest personAddRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage);
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryId.ToString()
            });
            return View();
        }
        PersonResponse response = _personsService.AddPerson(personAddRequest);
        return RedirectToAction("Index", "Person");
    }
    #endregion

    #region EditPerson
    [HttpGet]
    [Route("[action]/{personId}")]
    public IActionResult Edit(Guid personId)
    {
        PersonResponse? person = _personsService.GetPersonByPersonId(personId);
        if (person is null)
            return RedirectToAction("Index", "Person");
        PersonUpdateRequest updatePerson = person.FromPersonResponseToPersonUpdateRequest();
        List<CountryResponse> countries = _countriesService.GetAllCountries();
        ViewBag.Countries = countries.Select(country => new SelectListItem()
        {
            Text = country.CountryName,
            Value = country.CountryId.ToString()
        });
        return View(updatePerson);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage);
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(country => new SelectListItem()
            {
                Text = country.CountryName,
                Value = country.CountryId.ToString()
            });
            return View(personUpdateRequest);
        }
        _personsService.UpdatePerson(personUpdateRequest);
        return RedirectToAction("Index", "Person");
    }
    #endregion

    #region DeletePerson
    [HttpGet]
    [Route("[action]/{personId}")]
    public IActionResult Delete(Guid personId)
    {
        PersonResponse? person = _personsService.GetPersonByPersonId(personId);
        if (person is null)
            return RedirectToAction("Index", "Person");
        return View(person);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
    {
        PersonResponse? person = _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
        if (person is null)
            return RedirectToAction("Index", "Person");
        _personsService.DeletePerson(personUpdateRequest.PersonId);
        return RedirectToAction("Index", "Person");
    }
    #endregion
}
