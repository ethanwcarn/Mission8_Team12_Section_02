// =============================================================================
// ICategoryRepository.cs – Category Repository Interface
// Defines the contract for category data access. Categories are read-only
// in this application (they are seeded in the database and not user-editable),
// so only retrieval methods are needed.
// =============================================================================

using Mission8.Models;

namespace Mission8.Data
{
    public interface ICategoryRepository
    {
        // Queryable collection of all categories, ordered alphabetically.
        // Useful for building custom queries if needed.
        IQueryable<Category> Categories { get; }

        // Returns all categories as a list, sorted alphabetically by name.
        // Used to populate the category dropdown on the Add/Edit form.
        IEnumerable<Category> GetAllCategories();
    }
}
