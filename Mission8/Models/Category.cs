// =============================================================================
// Category.cs – Category Entity Model
// Represents a task category (e.g., Home, School, Work, Church).
// Categories are stored in their own database table and linked to tasks
// via a one-to-many relationship (one category can have many tasks).
// Used to populate the category dropdown on the Add/Edit Task form.
// =============================================================================

using System.ComponentModel.DataAnnotations;

namespace Mission8.Models
{
    public class Category
    {
        // Primary key – unique identifier for each category
        [Key]
        public int CategoryId { get; set; }

        // The display name of the category (required, max 100 characters)
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        // Navigation property – collection of all tasks that belong to this category.
        // This enables Entity Framework to traverse the one-to-many relationship
        // from the Category side (one category → many tasks).
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
