using de.openelp.regatta.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace de.openelp.regatta.Services;

/// <summary>
/// Persists application configuration to a JSON file in the user's application data folder.
/// Sensitive data such as passwords are never written to disk.
/// </summary>
public sealed class FileConfigurationPersistence : IConfigurationPersistence
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    /// <summary>Sentinel value indicating that no event is selected.</summary>
    private const int NoEventSelected = -1;

    private readonly string _configDirectory;

    /// <summary>
    /// Initializes a new instance using the default application data folder.
    /// </summary>
    public FileConfigurationPersistence()
        : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LuebeckRegatta"))
    {
    }

    /// <summary>
    /// Initializes a new instance using the specified directory.
    /// This overload is intended for testing purposes.
    /// </summary>
    /// <param name="configDirectory">The directory in which the config file will be stored.</param>
    public FileConfigurationPersistence(string configDirectory)
    {
        _configDirectory = configDirectory;
    }

    private string ConfigFilePath => Path.Combine(_configDirectory, "config.json");

    /// <inheritdoc />
    public async Task LoadAsync(IAppConfiguration config)
    {
        try
        {
            if (!File.Exists(ConfigFilePath))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(ConfigFilePath).ConfigureAwait(false);
            var dto = JsonSerializer.Deserialize<ConfigurationFileDto>(json, SerializerOptions);
            if (dto == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(dto.WebApiBaseUrl))
            {
                config.WebApiBaseUrl = dto.WebApiBaseUrl;
            }

            config.SelectedEventId = dto.SelectedEventId;
            config.UserName = dto.UserName ?? string.Empty;
            config.IsDebugMode = dto.IsDebugMode;
            config.AppTheme = dto.AppTheme ?? string.Empty;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading configuration from file: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task SaveAsync(IAppConfiguration config)
    {
        try
        {
            Directory.CreateDirectory(_configDirectory);

            var dto = new ConfigurationFileDto
            {
                WebApiBaseUrl = config.WebApiBaseUrl,
                SelectedEventId = config.SelectedEventId,
                UserName = config.UserName,
                IsDebugMode = config.IsDebugMode,
                AppTheme = config.AppTheme
            };

            var json = JsonSerializer.Serialize(dto, SerializerOptions);
            await File.WriteAllTextAsync(ConfigFilePath, json).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving configuration to file: {ex.Message}");
        }
    }

    /// <summary>
    /// Data-transfer object used for JSON serialization.
    /// Does not include sensitive fields such as <c>Password</c> or <c>AuthToken</c>.
    /// </summary>
    private sealed class ConfigurationFileDto
    {
        /// <summary>Gets or sets the base URL of the Web API.</summary>
        [JsonPropertyName("webApiBaseUrl")]
        public string WebApiBaseUrl { get; set; } = string.Empty;

        /// <summary>Gets or sets the ID of the selected event.</summary>
        [JsonPropertyName("selectedEventId")]
        public int SelectedEventId { get; set; } = NoEventSelected;

        /// <summary>Gets or sets the stored username (non-sensitive).</summary>
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        /// <summary>Gets or sets whether debug mode is enabled.</summary>
        [JsonPropertyName("isDebugMode")]
        public bool IsDebugMode { get; set; }

        /// <summary>Gets or sets the selected application theme.</summary>
        [JsonPropertyName("appTheme")]
        public string? AppTheme { get; set; }
    }
}
