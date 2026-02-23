// =============================================================================
// JARED - Assignment #1: Models / Database / Setup
// Purpose: Repository implementation for Task operations (Repository Pattern).
// =============================================================================

using Microsoft.EntityFrameworkCore;
using Mission8.Models;

namespace Mission8.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Mission8Context _context;

        public TaskRepository(Mission8Context context)
        {
            _context = context;
        }

        public IQueryable<Task> Tasks => _context.Tasks.Include(t => t.Category);

        public IEnumerable<Task> GetIncompleteTasks()
        {
            return _context.Tasks
                .Include(t => t.Category)
                .Where(t => !t.Completed)
                .OrderBy(t => t.Quadrant)
                .ThenBy(t => t.DueDate);
        }

        public Task? GetTaskById(int taskId)
        {
            return _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
        }

        public void AddTask(Task task)
        {
            _context.Tasks.Add(task);
            SaveTask();
        }

        public void UpdateTask(Task task)
        {
            _context.Tasks.Update(task);
            SaveTask();
        }

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

        public void SaveTask()
        {
            _context.SaveChanges();
        }
    }
}
