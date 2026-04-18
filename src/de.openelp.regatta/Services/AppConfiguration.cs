using System;
using de.openelp.regatta.Interfaces;

namespace de.openelp.regatta.Services;

/// <summary>
/// Stores central app configuration values that are needed across services and view models.
/// </summary>
public sealed class AppConfiguration : IAppConfiguration
{
    private string _webApiBaseUrl = "https://regatta-test.grinch-tech.de";

    /// <summary>
    /// Gets the shared app-wide configuration instance.
    /// </summary>
    public static AppConfiguration Current { get; } = new();

    /// <summary>
    /// Gets or sets the base URL of the Web API.
    /// </summary>
    public string WebApiBaseUrl
    {
        get => _webApiBaseUrl;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Web API base URL must not be empty.", nameof(value));
            }

            _webApiBaseUrl = value.TrimEnd('/');
        }
    }

    /// <summary>
    /// Gets or sets the currently selected event ID.
    /// </summary>
    public int SelectedEventId { get; set; }

    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    public string UserName { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the password for authentication.
    /// </summary>
    public string Password { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the authentication token for API requests.
    /// </summary>
    public string AuthToken { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the app is running in debug mode.
    /// </summary>
    public bool IsDebugMode { get; set; }

    /// <summary>
    /// Gets or sets the app theme (e.g., "Light", "Dark", "System").
    /// </summary>
    public string AppTheme { get; set; } = String.Empty;
}