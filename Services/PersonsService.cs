using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
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
    }
}
