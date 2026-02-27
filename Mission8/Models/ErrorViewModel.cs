// =============================================================================
// ErrorViewModel.cs – Error View Model
// Used by the shared Error view (Views/Shared/Error.cshtml) to display
// error details. ASP.NET Core passes a RequestId so developers can trace
// the failing request in logs. In production, only the RequestId is shown
// (no stack traces or sensitive details).
// =============================================================================

namespace Mission8.Models
{
    public class ErrorViewModel
    {
        // The unique identifier for the HTTP request that caused the error.
        // Useful for correlating errors in server logs.
        public string? RequestId { get; set; }

        // Returns true when a RequestId exists, controlling whether
        // the Error view displays the "Request ID" section.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
