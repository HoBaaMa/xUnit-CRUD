using Entities;

namespace ServiceContracts.DTOs
{
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
