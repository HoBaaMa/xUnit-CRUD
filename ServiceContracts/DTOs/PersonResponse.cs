using Entities;

namespace ServiceContracts.DTOs
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string? PersonName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Age { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Compare the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The PersonResponse Object to compare</param>
        /// <returns>True or false, indicating whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            return (Id == person.Id && PersonName == person.PersonName && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">The Person object to convert</param>
        /// <returns>Returns the converted PersonResponse object</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                Id = person.Id,
                PersonName = person.PersonName,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                CountryId = person.CountryId,
                Gender = person.Gender,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
