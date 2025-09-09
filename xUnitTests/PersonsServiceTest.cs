using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services;
using Xunit.Abstractions;

namespace xUnitTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        private List<PersonResponse> AddPersons()
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

            foreach (PersonAddRequest person in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(person);
                personResponsesFromAdd.Add(personResponse);
            }
            return personResponsesFromAdd;
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
        // The GetAllPersons() should return an empty list by default
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
            List<PersonResponse> personResponsesFromAdd = AddPersons();

            // Print personResponsesFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person in personResponsesFromAdd)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            // Act
            List<PersonResponse> personResponsesFromGet = _personService.GetAllPersons();

            // Assert
            foreach(PersonResponse person in personResponsesFromAdd)
            {
                Assert.Contains(person, personResponsesFromGet);
            }

            // Print personResponsesFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponsesFromGet)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

        }
        #endregion

        #region GetFilteredPersons Tests
        // The GetFilteredPersons() should return an empty list by default
        [Fact]
        public void GetFilteredPersons_EmptyList()
        {
            // Act
            List<PersonResponse> personResponses = _personService.GetFilteredPersons("","");

            // Assert
            Assert.Empty(personResponses);
        }

        // If the search text is empty and search by is "Person Name", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            List<PersonResponse> personResponsesFromAdd = AddPersons();

            // Print personResponsesFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            // Act
            List<PersonResponse> personResponsesFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            // Assert
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                Assert.Contains(person, personResponsesFromSearch);
            }

            // Print personResponsesFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponsesFromSearch)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

        }

        // Fisrt we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            List<PersonResponse> personResponsesFromAdd = AddPersons();

            // Print personResponsesFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                if (person.PersonName is not null)
                {
                    if (person.PersonName.Contains("do", StringComparison.OrdinalIgnoreCase))
                    {
                        _testOutputHelper.WriteLine(person.ToString());
                    }
                }
            }

            // Act
            List<PersonResponse> personResponsesFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "Do");

            // Assert
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                if (person.PersonName is not null)
                {
                    if (person.PersonName.Contains("do", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person, personResponsesFromSearch);
                    }
                }
            }

            // Print personResponsesFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponsesFromSearch)
            {
                if (person.PersonName is not null)
                {
                    if (person.PersonName.Contains("do", StringComparison.OrdinalIgnoreCase))
                    {
                        _testOutputHelper.WriteLine(person.ToString());
                    }
                }
            }

        }
        #endregion

        #region GetSortedPersons Tests
        // When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public void GetSortedPersons_SortByPersonName()
        {
            List<PersonResponse> personResponsesFromAdd = AddPersons();

            // Print personResponsesFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            // Act
            List<PersonResponse> personResponsesFromSort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), ServiceContracts.Enums.SortOrderOptions.DESC);

            // ✅ Sort the expected list BEFORE assertion
            personResponsesFromAdd = personResponsesFromAdd.OrderByDescending(p => p.PersonName).ToList();

            // Assert - Now both lists are in the same order
            for (int i = 0; i < personResponsesFromAdd.Count; i++)
            {
                Assert.Equal(personResponsesFromAdd[i], personResponsesFromSort[i]);
            }

            // Print results
            _testOutputHelper.WriteLine("Expected (after sorting):");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
                        
            _testOutputHelper.WriteLine("Actual:");

        }

        #endregion

        #region UpdatePerson Tests
        // When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullUpdateRequest()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        // Wheb we supply invalid person ID, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            // Arrange
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest { Id = Guid.NewGuid() };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        // When PersonName is null, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest { PersonName = "Joe", Address = "New York 2941", CountryId = countryResponse.Id, DateOfBirth = DateTime.Parse("2001-09-01"), Email = "joe@mail.com", Gender = ServiceContracts.Enums.GenderOptions.Male, ReceiveNewsLetters = false };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdate();
            personUpdateRequest.PersonName = null;


            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        // First, add new person and try to update the person name and email
        [Fact]
        public void UpdatePerson_ProperPersonUpdateRequest()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest { PersonName = "Joe", Address = "New York 2941", CountryId = countryResponse.Id, DateOfBirth = DateTime.Parse("2001-09-01"), Email = "joe@mail.com", Gender = ServiceContracts.Enums.GenderOptions.Male, ReceiveNewsLetters = false };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdate();
            personUpdateRequest.PersonName = "Mark";
            personUpdateRequest.Email = "mark.mail@gmail.com";

            // Act 
            PersonResponse updatePerson = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? getPerson = _personService.GetPersonById(personUpdateRequest.Id);

            // Assert
            Assert.Equal(getPerson, updatePerson);
        }

        #endregion
    }
}
