// =============================================================================
// CategoryRepository.cs – Category Repository Implementation
// Implements ICategoryRepository using Entity Framework Core.
// Provides read-only access to the Categories table for populating
// dropdown menus in the task form.
// =============================================================================

using Mission8.Models;

namespace Mission8.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        // The EF Core database context, injected via constructor injection
        private readonly Mission8Context _context;

        // Constructor – receives the DbContext from the DI container
        public CategoryRepository(Mission8Context context)
        {
            _context = context;
        }

        // Returns all categories as an IQueryable, ordered alphabetically.
        // IQueryable defers execution until the data is actually iterated.
        public IQueryable<Category> Categories => _context.Categories.OrderBy(c => c.CategoryName);

        // Returns all categories as a materialized List, ordered alphabetically.
        // ToList() executes the query immediately so the data is ready for the view.
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.CategoryName).ToList();
        }
    }
}
