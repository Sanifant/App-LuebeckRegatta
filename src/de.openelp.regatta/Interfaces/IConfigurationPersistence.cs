using System.Threading.Tasks;

namespace de.openelp.regatta.Interfaces;

/// <summary>
/// Provides platform-specific methods to load and save application configuration persistently.
/// </summary>
public interface IConfigurationPersistence
{
    /// <summary>
    /// Loads previously persisted configuration values into the provided configuration instance.
    /// </summary>
    /// <param name="config">The configuration instance to populate.</param>
    Task LoadAsync(IAppConfiguration config);

    /// <summary>
    /// Persists the current configuration values from the provided configuration instance.
    /// Password is never written to persistent storage.
    /// </summary>
    /// <param name="config">The configuration instance to read from.</param>
    Task SaveAsync(IAppConfiguration config);
}
