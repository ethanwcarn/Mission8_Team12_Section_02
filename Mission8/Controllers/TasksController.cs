// =============================================================================
// TasksController.cs – Task CRUD Controller
// Handles all task-related actions: viewing the Quadrants page, adding a new
// task, editing an existing task, deleting a task, and marking a task complete.
// Uses the Repository Pattern – interacts with ITaskRepository and
// ICategoryRepository instead of the DbContext directly.
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Mission8.Data;
// Alias to avoid naming conflict with System.Threading.Tasks.Task
using MissionTask = Mission8.Models.Task;

namespace Mission8.Controllers
{
    public class TasksController : Controller
    {
        // Repository dependencies injected via constructor injection.
        // _taskRepository handles all task database operations.
        // _categoryRepository provides category data for dropdown menus.
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;

        // Constructor – ASP.NET Core's DI container automatically provides
        // the repository instances registered in Program.cs.
        public TasksController(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: /Tasks/Quadrants
        // Retrieves all incomplete tasks from the database and passes them
        // to the Quadrants view, which displays them in a 2x2 grid layout
        // based on Stephen Covey's time management matrix.
        [HttpGet]
        public IActionResult Quadrants()
        {
            var tasks = _taskRepository.GetIncompleteTasks();
            return View(tasks);
        }

        // Helper method that loads all categories into ViewBag so the
        // Add/Edit form can populate its category dropdown menu.
        // Called before rendering the AddEdit view.
        private void PopulateCategories()
        {
            ViewBag.Categories = _categoryRepository.GetAllCategories();
        }

        // GET: /Tasks/AddEdit/{id?}
        // If no id is provided → shows a blank form to create a new task.
        // If an id is provided → loads the existing task for editing.
        // Returns 404 Not Found if the provided id doesn't match any task.
        [HttpGet]
        public IActionResult AddEdit(int? id)
        {
            PopulateCategories();

            if (!id.HasValue)
            {
                // No ID means the user wants to create a new task
                var newTask = new MissionTask();
                return View(newTask);
            }

            // ID provided – look up the existing task for editing
            var existingTask = _taskRepository.GetTaskById(id.Value);
            if (existingTask == null)
            {
                return NotFound();
            }

            return View(existingTask);
        }

        // POST: /Tasks/AddEdit
        // Receives the submitted form data. If validation fails, redisplays
        // the form with error messages. Otherwise, either inserts a new task
        // (TaskId == 0) or updates an existing one, then redirects to Quadrants.
        // [ValidateAntiForgeryToken] protects against cross-site request forgery attacks.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(MissionTask task)
        {
            if (!ModelState.IsValid)
            {
                // Validation failed – reload categories and show the form again
                PopulateCategories();
                return View(task);
            }

            if (task.TaskId == 0)
            {
                // TaskId is 0 → this is a brand-new task, so insert it
                _taskRepository.AddTask(task);
            }
            else
            {
                // TaskId is non-zero → this is an existing task, so update it
                _taskRepository.UpdateTask(task);
            }

            // After saving, redirect to the Quadrants view to see the updated list
            return RedirectToAction(nameof(Quadrants));
        }

        // GET: /Tasks/DeleteTask/{id}
        // Finds the task by ID and permanently removes it from the database.
        // Returns 404 if the task doesn't exist. After deletion, redirects
        // back to the Quadrants view.
        [HttpGet]
        public IActionResult DeleteTask(int id)
        {
            var existingTask = _taskRepository.GetTaskById(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            _taskRepository.DeleteTask(id);
            return RedirectToAction(nameof(Quadrants));
        }

        // POST: /Tasks/MarkComplete/{id}
        // Sets the task's Completed flag to true so it no longer appears
        // in the Quadrants view. Uses POST to prevent accidental completion
        // via URL manipulation. Redirects back to Quadrants afterward.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkComplete(int id)
        {
            _taskRepository.MarkTaskComplete(id);
            return RedirectToAction(nameof(Quadrants));
        }
    }
}
