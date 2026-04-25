using de.openelp.regatta.Models;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides central runtime configuration values shared across the app.
/// </summary>
public interface IAppConfiguration
{
    /// <summary>
    /// Gets or sets the base URL of the Web API.
    /// </summary>
    string WebApiBaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the currently selected event ID.
    /// </summary>
    int SelectedEventId { get; set; }

    /// <summary>
    /// Gets or sets the username of the currently logged-in user (if applicable).
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password of the currently logged-in user (if applicable).
    /// </summary>
    string Password { get; set; }

    /// <summary>
    /// Gets or sets the authentication token for API requests (if applicable).
    /// </summary>
    string AuthToken { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the app is in debug mode.
    /// </summary>
    bool IsDebugMode { get; set; }

    /// <summary>
    /// Gets or sets the app's current theme (e.g., "Light", "Dark
    /// </summary>
    string AppTheme { get; set; }

    /// <summary>
    /// Gets or sets the selected event.
    /// </summary>
    /// <value>
    /// The selected event.
    /// </value>
    EventModel SelectedEvent { get; set; }
    UserDto? User { get; }
}