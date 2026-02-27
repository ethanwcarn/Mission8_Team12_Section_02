// =============================================================================
// Task.cs – Task Entity Model
// Represents a single task in the time management matrix. Each task belongs to
// one of four Covey Quadrants and is linked to a Category (Home, School, etc.).
// Entity Framework maps this class to the "Tasks" table in the SQLite database.
// =============================================================================

using System.ComponentModel.DataAnnotations;

namespace Mission8.Models
{
    public class Task
    {
        // Primary key – auto-incremented unique identifier for each task
        [Key]
        public int TaskId { get; set; }

        // The name/description of the task (required, max 200 characters).
        // If left blank, the validation error message is displayed to the user.
        [Required(ErrorMessage = "Task name is required.")]
        [StringLength(200)]
        public string TaskName { get; set; } = string.Empty;

        // Optional due date for the task. Nullable because not every task has a deadline.
        public DateTime? DueDate { get; set; }

        // The Covey Quadrant (1–4) that this task belongs to:
        //   1 = Urgent & Important
        //   2 = Not Urgent & Important
        //   3 = Urgent & Not Important
        //   4 = Not Urgent & Not Important
        [Required(ErrorMessage = "Quadrant is required.")]
        [Range(1, 4, ErrorMessage = "Quadrant must be between 1 and 4.")]
        public int Quadrant { get; set; }

        // Foreign key linking this task to a Category (e.g., Home, School, Work, Church).
        // Required so every task must be categorized.
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        // Navigation property – allows Entity Framework to load the related
        // Category object. Nullable because EF may not always eager-load it.
        public Category? Category { get; set; }

        // Tracks whether the task has been marked as done.
        // Completed tasks are hidden from the Quadrants view.
        public bool Completed { get; set; }
    }
}
