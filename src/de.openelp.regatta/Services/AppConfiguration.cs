using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;

namespace de.openelp.regatta.Services;

/// <summary>
/// Stores central app configuration values that are needed across services and view models.
/// </summary>
public sealed class AppConfiguration : IAppConfiguration
{
    private string _webApiBaseUrl = "https://regatta-test.grinch-tech.de";
    private EventModel? _eventModel;
    private int selectedEventId = -1;
    private UserDto? _user;

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
    public int SelectedEventId
    {
        get
        {
            return selectedEventId;
        }
        set
        {
            selectedEventId = value;
        }
    }

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

    /// <summary>
    /// Gets or sets the selected event.
    /// </summary>
    /// <value>
    /// The selected event.
    /// </value>
    public EventModel SelectedEvent
    {
        get
        {
            if(_eventModel == null && selectedEventId != -1)
            { 
                var apiService = App.Services.GetService<IEventApiService>();
                if (apiService != null)
                {
                    _eventModel = apiService.GetEventAsync(selectedEventId).Result;
                }
            }
            return _eventModel;
        }
        set
        {
            this._eventModel = value;
            this.selectedEventId = value.Id;
        }
    }

    public UserDto? User
    {
        get
        {
            if (_user == null)
            {
                if (string.IsNullOrWhiteSpace(this.UserName) && string.IsNullOrWhiteSpace(this.Password))
                {
                    return null;
                }

                var _authApiService = App.Services.GetService<IAuthApiService>();

                _authApiService.LoginAsync(new LoginRequestDto() { Username = this.UserName, Password = this.Password })
                    .ContinueWith(task =>
                    {

                        if (task.IsCompletedSuccessfully)
                        {

                            this._user = task.Result;
                        }
                    });
            }

            return _user;
        }
    }
}