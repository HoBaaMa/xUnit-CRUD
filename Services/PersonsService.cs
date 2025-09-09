using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;
        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            // Convert the Person Object into PersonResponse type
            PersonResponse personResponse = person.ToPersonResponse();

            //CountryResponse countryResponse = _countriesService.GetCountryById(person.CountryId);
            personResponse.CountryName = _countriesService.GetCountryById(person.CountryId)?.CountryName;

            return personResponse;
        }

        

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            // Check if PersonAddRequest is not null
            if (personAddRequest is null) throw new ArgumentNullException(nameof(PersonAddRequest));

            // Model Validation
            ValidationHelper.ModelValidation(personAddRequest);

            // Convert PersonAddRequest into Person type
            Person person = personAddRequest.ToPerson();
            // Generate PersonID
            person.Id = Guid.NewGuid();

            // Add person object to persons list
            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonById(Guid? id)
        {
            if (id is null) return null;

            return _persons.FirstOrDefault(p => p.Id == id)?.ToPersonResponse();
        }

        // *** WE CAN REDUCE REPETITIVE CODE BY USING REFLACTION! ***

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchText)
        {
            List<PersonResponse> personResponses = GetAllPersons();
            if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchText)) return personResponses;

            //List<PersonResponse> matchingPersons = personResponses.W
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    personResponses = personResponses.Where(p =>
                    (!string.IsNullOrEmpty(p.PersonName) ?
                    p.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    personResponses = personResponses.Where(p =>
                    (!string.IsNullOrEmpty(p.Email) ?
                    p.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    personResponses = personResponses.Where(p =>
                    (p.DateOfBirth != null) ? 
                    p.DateOfBirth.Value.ToString("ddd mm yyyy").Contains(searchText, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    personResponses = personResponses.Where(p =>
                    (!string.IsNullOrEmpty(p.Gender)) ? 
                    p.Gender.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.CountryId):
                    personResponses = personResponses.Where(p =>
                    (!string.IsNullOrEmpty(p.CountryName)) ? 
                    p.CountryName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Address):
                    personResponses = personResponses.Where(p =>
                    (!string.IsNullOrEmpty(p.Address)) ? 
                    p.Address.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                
            }

            return personResponses;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
            {
                (nameof(Person.PersonName), SortOrderOptions.ASC) 
                => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(Person.PersonName), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(Person.Email), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(Person.Email), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(Person.DateOfBirth), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(Person.DateOfBirth), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null) throw new ArgumentNullException(nameof(personUpdateRequest));

            // Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            // Get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(p => p.Id == personUpdateRequest.Id);

            if (matchingPerson is null) throw new ArgumentException("Given person ID doesn't exist");

            // Update all details
            matchingPerson.PersonName = matchingPerson.PersonName;
            matchingPerson.Email = matchingPerson.Email;
            matchingPerson.Address = matchingPerson.Address;
            matchingPerson.Gender = matchingPerson.Gender;
            matchingPerson.DateOfBirth = matchingPerson.DateOfBirth;
            matchingPerson.ReceiveNewsLetters = matchingPerson.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }
    }
}
