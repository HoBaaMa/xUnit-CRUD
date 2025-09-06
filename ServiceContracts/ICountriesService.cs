using ServiceContracts.DTOs;

namespace ServiceContracts
{

    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService 
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>The country object after adding it (including newly generated country ID)</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);


        /// <summary>
        /// Returns all the countries from the list
        /// </summary>
        /// <returns>All countries from the list as list of CountryResponse</returns>
        List<CountryResponse> GetAllCountries();


        /// <summary>
        /// Returns a country response object based on the given country ID
        /// </summary>
        /// <param name="id">CountryID (Guid) to get</param>
        /// <returns>Matching country as CountryResponse object</returns>
        CountryResponse? GetCountryById(Guid? id);
    }
}
