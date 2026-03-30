using System;

namespace de.openelp.regatta.Services;

/// <summary>
/// Stores central app configuration values that are needed across services and view models.
/// </summary>
public sealed class AppConfiguration : IAppConfiguration
{
    private string _webApiBaseUrl = "https://frgle";

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
}