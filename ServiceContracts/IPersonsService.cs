using ServiceContracts.DTOs;
using ServiceContracts.Enums;

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
        /// <summary>
        /// Retrieves a list of persons filtered based on the specified criteria.
        /// </summary>
        /// <param name="searchBy">The field to filter by. This can be a property name such as "Name" or "Age".</param>
        /// <param name="searchText">The value to filter against. If null or empty, no filtering is applied.</param>
        /// <returns>A list of <see cref="PersonResponse"/> objects that match the specified filter criteria. If no matches are
        /// found, an empty list is returned.</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchText);
        /// <summary>
        /// Retrieves a sorted list of persons based on the specified property and sort order.
        /// </summary>
        /// <param name="allPersons">The list of persons to be sorted. Cannot be null.</param>
        /// <param name="sortBy">The property name to sort by. For example, "Name" or "Age".</param>
        /// <param name="sortOrder">The order in which to sort the list. Use <see cref="SortOrderOptions.Ascending"/> for ascending order or
        /// <see cref="SortOrderOptions.Descending"/> for descending order.</param>
        /// <returns>A sorted list of persons. If <paramref name="allPersons"/> is empty, an empty list is returned.</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        /// <summary>
        /// Updates the details of an existing person based on the provided update request.
        /// </summary>
        /// <param name="personUpdateRequest">The request containing the updated information for the person.  This parameter cannot be null.</param>
        /// <returns>A <see cref="PersonResponse"/> object containing the updated details of the person.</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="id">PersonID to delete</param>
        /// <returns>True, if the deletion is successful; otherwise false</returns>
        bool DeletePerson(Guid? id);
    }
}
