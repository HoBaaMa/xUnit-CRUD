using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            #region Validations
            // Check if the CountryAddRequest is null
            if (countryAddRequest is null) throw new ArgumentNullException(nameof(countryAddRequest));

            // Check if the CountryName is null
            if (countryAddRequest.CountryName is null) throw new ArgumentException(nameof(countryAddRequest.CountryName));

            // Check if the CountryName is duplicate
            if (_countries.Any(c => c.CountryName!.ToLower().Equals(countryAddRequest.CountryName.ToLower()))) throw new ArgumentException(nameof(countryAddRequest.CountryName));
            #endregion

            // Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            // Generate CountryID
            country.Id = Guid.NewGuid();

            // Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? id)
        {
            if (id is null) return null;

            Country? result = _countries.FirstOrDefault(c => c.Id == id);
            if (result is null) return null;

            return new CountryResponse { CountryName = result.CountryName, Id = result.Id };
        }
    }
}
