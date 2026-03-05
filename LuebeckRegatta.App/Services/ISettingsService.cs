namespace LuebeckRegatta.App.Services;

/// <summary>
/// Service interface for managing application settings
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Gets or sets the News RSS Feed URL
    /// </summary>
    string NewsFeedUrl { get; set; }

    /// <summary>
    /// Gets or sets the Regatta API Base URL
    /// </summary>
    string RegattaApiUrl { get; set; }

    /// <summary>
    /// Gets or sets the Regatta API Username
    /// </summary>
    string RegattaUsername { get; set; }

    /// <summary>
    /// Gets or sets the Regatta API Password
    /// </summary>
    string RegattaPassword { get; set; }

    /// <summary>
    /// Gets or sets the selected Event ID
    /// </summary>
    int? SelectedEventId { get; set; }

    /// <summary>
    /// Resets all settings to default values
    /// </summary>
    void ResetToDefaults();
}
