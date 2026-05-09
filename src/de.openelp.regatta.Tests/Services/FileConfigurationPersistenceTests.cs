using System;
using System.IO;
using System.Threading.Tasks;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using Xunit;

namespace de.openelp.regatta.Tests.Services;

/// <summary>
/// Tests for <see cref="FileConfigurationPersistence"/>.
/// </summary>
public class FileConfigurationPersistenceTests : IDisposable
{
    private readonly string _tempDir;

    public FileConfigurationPersistenceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "LuebeckRegattaTests_" + Guid.NewGuid());
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, recursive: true);
        }
    }

    private static StubAppConfiguration CreateConfig() => new();

    /// <summary>
    /// Verifies that loading from a non-existent file leaves configuration unchanged.
    /// </summary>
    [Fact]
    public async Task Load_WhenFileDoesNotExist_LeavesConfigurationUnchanged()
    {
        var persistence = new FileConfigurationPersistence(_tempDir);
        var config = CreateConfig();

        await persistence.LoadAsync(config);

        Assert.Equal("https://default.example.com", config.WebApiBaseUrl);
        Assert.Equal(-1, config.SelectedEventId);
        Assert.Equal(string.Empty, config.UserName);
        Assert.False(config.IsDebugMode);
        Assert.Equal(string.Empty, config.AppTheme);
    }

    /// <summary>
    /// Verifies that a saved configuration is correctly restored after loading.
    /// </summary>
    [Fact]
    public async Task SaveAndLoad_RoundTrip_RestoresNonSensitiveValues()
    {
        var persistence = new FileConfigurationPersistence(_tempDir);
        var saved = CreateConfig();
        saved.WebApiBaseUrl = "https://api.example.com";
        saved.SelectedEventId = 42;
        saved.UserName = "testuser";
        saved.IsDebugMode = true;
        saved.AppTheme = "Dark";
        saved.Password = "secret";

        await persistence.SaveAsync(saved);

        var loaded = CreateConfig();
        await persistence.LoadAsync(loaded);

        Assert.Equal("https://api.example.com", loaded.WebApiBaseUrl);
        Assert.Equal(42, loaded.SelectedEventId);
        Assert.Equal("testuser", loaded.UserName);
        Assert.True(loaded.IsDebugMode);
        Assert.Equal("Dark", loaded.AppTheme);
    }

    /// <summary>
    /// Verifies that <c>Password</c> is never written to the config file.
    /// </summary>
    [Fact]
    public async Task Save_DoesNotPersistPassword()
    {
        var persistence = new FileConfigurationPersistence(_tempDir);
        var config = CreateConfig();
        config.Password = "supersecret";

        await persistence.SaveAsync(config);

        var configFilePath = Path.Combine(_tempDir, "config.json");
        var json = await File.ReadAllTextAsync(configFilePath, TestContext.Current.CancellationToken);
        Assert.DoesNotContain("supersecret", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("password", json, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifies that <c>AuthToken</c> is never written to the config file.
    /// </summary>
    [Fact]
    public async Task Save_DoesNotPersistAuthToken()
    {
        var persistence = new FileConfigurationPersistence(_tempDir);
        var config = CreateConfig();
        config.AuthToken = "mytoken123";

        await persistence.SaveAsync(config);

        var configFilePath = Path.Combine(_tempDir, "config.json");
        var json = await File.ReadAllTextAsync(configFilePath, TestContext.Current.CancellationToken);
        Assert.DoesNotContain("mytoken123", json, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifies that loading from a corrupt JSON file does not throw.
    /// </summary>
    [Fact]
    public async Task Load_CorruptFile_DoesNotThrow()
    {
        var configFilePath = Path.Combine(_tempDir, "config.json");
        await File.WriteAllTextAsync(configFilePath, "{ this is not valid json %%% }", TestContext.Current.CancellationToken);

        var persistence = new FileConfigurationPersistence(_tempDir);
        var config = CreateConfig();

        // Should not throw
        await persistence.LoadAsync(config);
    }

    /// <summary>
    /// Stub implementation of <see cref="IAppConfiguration"/> for testing.
    /// </summary>
    private sealed class StubAppConfiguration : IAppConfiguration
    {
        public string WebApiBaseUrl { get; set; } = "https://default.example.com";
        public int SelectedEventId { get; set; } = -1;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public bool IsDebugMode { get; set; }
        public string AppTheme { get; set; } = string.Empty;
        public EventModel SelectedEvent { get; set; } = null!;
        public UserDto? User => null;
    }
}
