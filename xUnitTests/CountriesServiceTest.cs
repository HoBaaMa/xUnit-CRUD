using ServiceContracts;
using Services;
using ServiceContracts.DTOs;
namespace xUnitTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry Tests
        // When CountryAddRequest is null, throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is null, throw ArgumentException

        [Fact]
        public void AddCountry_NullCountryName()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is duplicate, throw ArgumentException

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        // When you supply proper CountryName, insert the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japen" };

            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(request);
            List<CountryResponse> countriesResponse = _countriesService.GetAllCountries();

            // Assert
            Assert.True(countryResponse.Id != Guid.Empty);
            Assert.Contains(countryResponse, countriesResponse);
        }
        #endregion

        #region GetAllCountries Test
        [Fact]
        // The list of countries should be empty by default (before adding any countries)
        public void GetAllCountries_Empty()
        {
            // Act
            List<CountryResponse> CountriesResponse = _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(CountriesResponse);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> countriesAdd = new List<CountryAddRequest>()
            {
                new CountryAddRequest()
                {
                    CountryName = "USA"
                },
                new CountryAddRequest()
                {
                    CountryName = "UK"
                }
            };

            // Act
            List<CountryResponse> countriesResponse = new List<CountryResponse>();
            foreach(CountryAddRequest countryAdd in countriesAdd)
            {
               countriesResponse.Add(_countriesService.AddCountry(countryAdd));
            }

            List<CountryResponse> actualCountriesResponse = _countriesService.GetAllCountries();
            // Read each elemnt from countriesResponse
            foreach (CountryResponse expected_country in countriesResponse)
            {
                Assert.Contains(expected_country, actualCountriesResponse);
            }
        }

        #endregion
    }
}
