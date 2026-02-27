// =============================================================================
// Program.cs – Application Entry Point
// This is the main startup file for the ASP.NET Core MVC application.
// It configures services (dependency injection), middleware, and routing.
// =============================================================================

using Microsoft.EntityFrameworkCore;
using Mission8.Data;

// Create a WebApplication builder that will be used to configure services and middleware
var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// SERVICE REGISTRATION (Dependency Injection Container)
// Services registered here are available throughout the application via
// constructor injection. This is where we wire up the database, repositories,
// and MVC framework services.
// =============================================================================

// Register the Entity Framework Core DbContext with a SQLite database provider.
// The connection string is read from appsettings.json under "Mission8Connection".
// If the connection string is missing, it falls back to "Data Source=Mission8.db".
builder.Services.AddDbContext<Mission8Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Mission8Connection")
                      ?? "Data Source=Mission8.db"));

// Register the Task repository with a "scoped" lifetime, meaning one instance
// is created per HTTP request. This follows the Repository Pattern to abstract
// data access logic away from controllers.
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Register the Category repository (scoped) so controllers can retrieve
// category data (Home, School, Work, Church) for dropdown menus.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Add MVC services (controllers + Razor views) to the dependency injection container.
builder.Services.AddControllersWithViews();

// Build the configured application
var app = builder.Build();

// =============================================================================
// MIDDLEWARE PIPELINE CONFIGURATION
// Middleware runs in order for every HTTP request. The order matters:
// error handling → HTTPS → routing → auth → static files → endpoints.
// =============================================================================

// In non-development environments, use a generic error handler page
// and enable HTTP Strict Transport Security (HSTS) for security.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS for secure communication
app.UseHttpsRedirection();

// Enable routing so the app can match incoming URLs to controller actions
app.UseRouting();

// Enable authorization middleware (required even if no auth policies are configured)
app.UseAuthorization();

// Serve static files from the wwwroot folder (CSS, JS, images, libraries)
app.MapStaticAssets();

// =============================================================================
// DATABASE INITIALIZATION
// On application startup, ensure the SQLite database file exists and that
// all tables and seed data defined in Mission8Context are created.
// This eliminates the need to run migrations manually during development.
// =============================================================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Mission8Context>();
    dbContext.Database.EnsureCreated();
}

// =============================================================================
// ROUTE CONFIGURATION
// Define the default URL pattern: {controller}/{action}/{id?}
// Example: /Tasks/AddEdit/3  →  TasksController.AddEdit(id: 3)
// Default route: Home/Index (the landing page)
// =============================================================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Start the application and begin listening for HTTP requests
app.Run();
