// =============================================================================
// TaskRepository.cs – Task Repository Implementation
// Implements the ITaskRepository interface using Entity Framework Core.
// All database operations for tasks (CRUD + mark complete) are handled here.
// The controller never touches the DbContext directly – it goes through
// this repository, keeping data access logic separated from business logic.
// =============================================================================

using Microsoft.EntityFrameworkCore;
using Mission8.Models;
using MissionTask = Mission8.Models.Task;

namespace Mission8.Data
{
    public class TaskRepository : ITaskRepository
    {
        // The EF Core database context, injected via constructor injection
        private readonly Mission8Context _context;

        // Constructor – receives the DbContext from the DI container
        public TaskRepository(Mission8Context context)
        {
            _context = context;
        }

        // Returns all tasks with their related Category eagerly loaded.
        // IQueryable allows additional filtering/sorting before execution.
        public IQueryable<MissionTask> Tasks => _context.Tasks.Include(t => t.Category);

        // Returns only incomplete tasks, sorted by quadrant (1–4) first,
        // then by due date within each quadrant (earliest dates first).
        // Include(t => t.Category) eager-loads category names for display.
        public IEnumerable<MissionTask> GetIncompleteTasks()
        {
            return _context.Tasks
                .Include(t => t.Category)
                .Where(t => !t.Completed)
                .OrderBy(t => t.Quadrant)
                .ThenBy(t => t.DueDate);
        }

        // Looks up a single task by its primary key (TaskId).
        // Returns null if no task with that ID exists.
        public MissionTask? GetTaskById(int taskId)
        {
            return _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
        }

        // Adds a brand-new task to the database and immediately saves changes
        public void AddTask(MissionTask task)
        {
            _context.Tasks.Add(task);
            SaveTask();
        }

        // Marks an existing task as modified in the EF change tracker
        // and immediately saves the updated values to the database
        public void UpdateTask(MissionTask task)
        {
            _context.Tasks.Update(task);
            SaveTask();
        }

        // Deletes a task by its ID. First checks if the task exists;
        // if not found, exits silently to avoid exceptions.
        public void DeleteTask(int taskId)
        {
            var existingTask = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (existingTask is null)
            {
                return;
            }

            _context.Tasks.Remove(existingTask);
            SaveTask();
        }

        // Marks a task as completed by setting Completed = true.
        // Completed tasks are filtered out of the Quadrants view.
        public void MarkTaskComplete(int taskId)
        {
            var existingTask = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (existingTask is null)
            {
                return;
            }

            existingTask.Completed = true;
            SaveTask();
        }

        // Persists all pending changes in the EF change tracker to the SQLite database
        public void SaveTask()
        {
            _context.SaveChanges();
        }
    }
}
