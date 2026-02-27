// =============================================================================
// Mission8Context.cs – Entity Framework Core Database Context
// This class is the bridge between the C# application and the SQLite database.
// It defines which tables exist (DbSets) and seeds the database with initial
// data so the app has categories and sample tasks on first run.
// =============================================================================

using Microsoft.EntityFrameworkCore;
using Mission8.Models;
using MissionTask = Mission8.Models.Task;

namespace Mission8.Data
{
    public class Mission8Context : DbContext
    {
        // Constructor receives configuration options (like the connection string)
        // from Program.cs via dependency injection and passes them to the base DbContext.
        public Mission8Context(DbContextOptions<Mission8Context> options) : base(options)
        {
        }

        // DbSet for the Tasks table – each MissionTask object maps to a row in this table.
        // "MissionTask" is an alias for Mission8.Models.Task (avoids conflict with System.Threading.Tasks.Task).
        public DbSet<MissionTask> Tasks => Set<MissionTask>();

        // DbSet for the Categories table – stores task categories (Home, School, Work, Church).
        public DbSet<Category> Categories => Set<Category>();

        // OnModelCreating is called once when EF Core builds the database model.
        // Here we seed the database with default categories and a few sample tasks
        // so the application has data to display immediately after setup.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed four default categories that appear in the dropdown menu
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Home" },
                new Category { CategoryId = 2, CategoryName = "School" },
                new Category { CategoryId = 3, CategoryName = "Work" },
                new Category { CategoryId = 4, CategoryName = "Church" }
            );

            // Seed three sample tasks spread across different quadrants and categories
            // so the Quadrants view is not empty on first launch
            modelBuilder.Entity<MissionTask>().HasData(
                new MissionTask
                {
                    TaskId = 1,
                    TaskName = "Finish Mission 8 database setup",
                    DueDate = new DateTime(2026, 2, 28),
                    Quadrant = 2,          // Not Urgent & Important
                    CategoryId = 2,        // School
                    Completed = false
                },
                new MissionTask
                {
                    TaskId = 2,
                    TaskName = "Plan weekly work priorities",
                    DueDate = new DateTime(2026, 3, 1),
                    Quadrant = 2,          // Not Urgent & Important
                    CategoryId = 3,        // Work
                    Completed = false
                },
                new MissionTask
                {
                    TaskId = 3,
                    TaskName = "Prepare family activity",
                    DueDate = null,        // No specific deadline
                    Quadrant = 4,          // Not Urgent & Not Important
                    CategoryId = 1,        // Home
                    Completed = false
                }
            );
        }
    }
}
