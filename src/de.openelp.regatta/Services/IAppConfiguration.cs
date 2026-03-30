namespace de.openelp.regatta.Services;

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
}