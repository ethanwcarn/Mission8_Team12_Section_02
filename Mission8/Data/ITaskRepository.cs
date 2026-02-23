// =============================================================================
// JARED - Assignment #1: Models / Database / Setup
// Purpose: Repository interface for Task operations (Repository Pattern).
// =============================================================================

using Mission8.Models;

namespace Mission8.Data
{
    public interface ITaskRepository
    {
        IQueryable<Task> Tasks { get; }

        IEnumerable<Task> GetIncompleteTasks();
        Task? GetTaskById(int taskId);
        void AddTask(Task task);
        void UpdateTask(Task task);
        void DeleteTask(int taskId);
        void MarkTaskComplete(int taskId);
        void SaveTask();
    }
}
