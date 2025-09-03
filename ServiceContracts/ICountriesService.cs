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
    }
}
