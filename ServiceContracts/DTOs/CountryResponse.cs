using Entities;
namespace ServiceContracts.DTOs
{
    /// <summary>
    /// DTO class that is used as return type for most of the CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string? CountryName { get; set; }

        // It compares the current object to another object of CountryResponse type and returns true, if both values are same; othwerwise returns true
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;

            CountryResponse countryResponse = (CountryResponse)obj;

            return this.Id == countryResponse.Id && this.CountryName == countryResponse.CountryName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    // Converts from Country object to CountryResponse object
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() { Id = country.Id, CountryName = country.CountryName };
        }
    }
}
