using ServiceContracts.DTOs;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person to the system and returns the result of the operation.
        /// </summary>
        /// <param name="person">The details of the person to add. This parameter cannot be null.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the result of the operation,  including the added person's
        /// details and any relevant status information.</returns>
        PersonResponse AddPerson(PersonAddRequest? person);
        /// <summary>
        /// Retrieves a list of all persons.
        /// </summary>
        /// <returns>A list of <see cref="PersonResponse"/> objects representing all persons.  The list will be empty if no
        /// persons are available.</returns>
        List<PersonResponse> GetAllPersons();
        /// <summary>
        /// Retrieves a person's details based on their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person to retrieve.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the details of the person  if found; otherwise, <see
        /// langword="null"/>.</returns>
        PersonResponse? GetPersonById(Guid? id);
    }
}
