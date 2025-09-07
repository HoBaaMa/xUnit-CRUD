using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services;
using System.Runtime.InteropServices;
using Xunit;
namespace xUnitTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        public PersonsServiceTest()
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
        }

        #region AddPerson Tests
        // When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            // Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        // When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_NullPersonName()
        {
            // Arrange 
            PersonAddRequest personAddRequest = new PersonAddRequest { PersonName = null };

            // Act
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        // When we supply proper person details, it should insert the person into the person list; and it should return an object of PersonResponse, which includes with the newly generated person ID
        [Fact]
        public void AddPerson_Proper()
        {
            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Doe", Email = "doe@gmail.com", Address = "New York 1246", DateOfBirth = DateTime.Parse("2001-09-10"), Gender = ServiceContracts.Enums.GenderOptions.Male, ReceiveNewsLetters = true, CountryId = Guid.NewGuid()};


            // Act
            PersonResponse person = _personService.AddPerson(personAddRequest);

            List<PersonResponse> personResponses = _personService.GetAllPersons();


            // Assert
            Assert.True(person.Id != Guid.Empty);

            Assert.Contains(person, personResponses);
        }

        #endregion

        #region GetPersonById Tests
        // If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public void GetPersonById_NullId()
        {
            // Arrange
            Guid? personId = null;

            // Act
            PersonResponse? personResponse =  _personService.GetPersonById(personId);

            // Assert
            Assert.Null(personResponse);
        }

        // If we supply a valid person ID, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonById_WithPersonId()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            // Act
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Doe",
                Email = "doe@mail.com",
                Address = "New York 1321",
                CountryId = countryResponse.Id,
                DateOfBirth = DateTime.Parse("2001-04-02"),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                ReceiveNewsLetters = false
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonResponse? personResponse1 = _personService.GetPersonById(personResponse.Id);

            // Assert
            Assert.Equal(personResponse, personResponse1);
        }
        #endregion

        #region GetAllPersons Tests
        // The GetAllPersons() should return an empty lis by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            // Act
            List<PersonResponse> personResponses = _personService.GetAllPersons();

            // Assert
            Assert.Empty(personResponses);
        }

        // First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            // Arrange 
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "Doe",
                Email = "doe@mail.com",
                Address = "New York 1321",
                CountryId = countryResponse.Id,
                DateOfBirth = DateTime.Parse("2001-04-02"),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                ReceiveNewsLetters = false
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest
            {
                PersonName = "Mark",
                Email = "mark@mail.com",
                Address = "UK 9421",
                CountryId = countryResponse1.Id,
                DateOfBirth = DateTime.Parse("2008-09-10"),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>
            {
                personAddRequest,
                personAddRequest2
            };
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();

            foreach(PersonAddRequest person in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(person);
                personResponsesFromAdd.Add(personResponse);
            }

            // Act
            List<PersonResponse> personResponsesFromGet = _personService.GetAllPersons();

            // Assert
            foreach(PersonResponse person in personResponsesFromAdd)
            {
                Assert.Contains(person, personResponsesFromGet);
            }
        }
        #endregion
    }
}
