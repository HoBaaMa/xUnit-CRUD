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
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() { Id = country.Id, CountryName = country.CountryName };
        }
    }
}
