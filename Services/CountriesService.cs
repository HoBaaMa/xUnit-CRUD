using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using System.Collections.Generic;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();

            if (initialize)
            {
                _countries.AddRange(new List<Country>() 
                {
                    new Country() { Id = Guid.Parse("ECA27C47-F6D9-4341-93C8-09351D80F11A"), CountryName = "Egypt" },
                    new Country() { Id = Guid.Parse("A63ADAAD-3BA2-4C1C-8662-B486BC214EE9"), CountryName = "UK" },
                    new Country() { Id = Guid.Parse("8AE16A78-29A1-4ED3-A9C8-262C8A3C3DB6"), CountryName = "USA" },
                    new Country() { Id = Guid.Parse("59943C29-6CDD-4A7F-846B-0256DF311739"), CountryName = "Germany" },
                    new Country() { Id = Guid.Parse("49C4DCCE-F5FB-4045-BAD5-34E8D33B89A5"), CountryName = "China" },
                });
                
            }
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
