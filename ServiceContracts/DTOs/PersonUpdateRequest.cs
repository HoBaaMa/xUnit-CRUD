using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTOs
{
    /// <summary>
    /// Represents a request to update the details of a person.
    /// </summary>
    /// <remarks>This class is used to encapsulate the data required to update a person's information. It
    /// includes properties for personal details such as name, email, date of birth, gender,  country, and address, as
    /// well as a flag indicating whether the person wishes to receive newsletters.</remarks>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person ID can't be blank.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Person Name cannot be blank.")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email address cannot be blank.")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email.")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>Person object</returns>
        public Person ToPerson()
        {
            return new Person
            {
                Id = Id,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
