// =============================================================================
// ITaskRepository.cs – Task Repository Interface
// Defines the contract (set of methods) that any Task repository must implement.
// Using an interface enables the Repository Pattern, which:
//   1. Decouples controllers from direct database access
//   2. Makes the code easier to unit test (you can swap in a mock repository)
//   3. Centralizes all task-related data operations in one place
// =============================================================================

using Mission8.Models;
using MissionTask = Mission8.Models.Task;

namespace Mission8.Data
{
    public interface ITaskRepository
    {
        // Queryable collection of all tasks (with Category loaded) for flexible LINQ queries
        IQueryable<MissionTask> Tasks { get; }

        // Returns only tasks that have not been marked as completed,
        // sorted by quadrant and then by due date
        IEnumerable<MissionTask> GetIncompleteTasks();

        // Finds and returns a single task by its primary key, or null if not found
        MissionTask? GetTaskById(int taskId);

        // Inserts a new task into the database
        void AddTask(MissionTask task);

        // Updates an existing task's properties in the database
        void UpdateTask(MissionTask task);

        // Removes a task from the database by its ID
        void DeleteTask(int taskId);

        // Sets a task's Completed flag to true so it no longer appears in the Quadrants view
        void MarkTaskComplete(int taskId);

        // Persists all pending changes to the database
        void SaveTask();
    }
}
