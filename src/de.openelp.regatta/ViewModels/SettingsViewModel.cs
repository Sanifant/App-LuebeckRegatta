using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace de.openelp.regatta.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly IAppConfiguration _configuration;
    private ObservableCollection<EventModel>? availableEvents;

    public SettingsViewModel(IAppConfiguration? configuration = null)
    {
        _configuration = configuration ?? AppConfiguration.Current;
        availableEvents = new ObservableCollection<EventModel>();
        this.AvailableThemes = new ObservableCollection<string>() { "Light", "Dark", "System" };

        if (!String.IsNullOrWhiteSpace(configuration.WebApiBaseUrl)) LoadEvents();

        UrlValid = "Yellow";
    }

    public bool CanCheckConnection
    {
        get
        {
            return !string.IsNullOrWhiteSpace(WebApiBaseUrl)
                && !string.IsNullOrWhiteSpace(UserName)
                && !string.IsNullOrWhiteSpace(Password);
        }
    }

    [RelayCommand]
    public void CheckConnection()
    {
        if (this._configuration.User == null)
        {
            this.CanSaveChanges = true;

            this.availableEvents = new ObservableCollection<EventModel>(this._configuration.User.Events);

            OnPropertyChanged(nameof(CanSaveChanges));
            UrlValid = "Green";
            ErrorMessage = string.Empty;
        }
        else
        {
            this.CanSaveChanges = false;
            UrlValid = "OrangeRed";
        }
    }

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public bool CanSaveChanges { get; set; }

    [ObservableProperty]
    private string urlValid = string.Empty;


    [RelayCommand]
    public Task SaveChanges()
    {

        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task ParseUrl()
    {
        if (WebApiBaseUrl != string.Empty)
        {
            var uri = new Uri(WebApiBaseUrl);
            var eventName = uri.Segments[1].TrimEnd('/');

            var _eventApiService = new EventApiService(this._configuration);
            var eventTask = _eventApiService.GetEventByNameAsync(eventName)
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        SelectedEvent = task.Result;
                    }
                    else
                    {
                        ErrorMessage = task.Exception?.InnerException?.Message ?? "unknown error";
                        this.CanSaveChanges = false;
                        UrlValid = "OrangeRed";
                    }
                });
        }


        return Task.CompletedTask;
    }

    private async void LoadEvents()
    {
        var _eventApiService = new EventApiService(this._configuration);
        try
        {
            this.availableEvents = new ObservableCollection<EventModel>(await _eventApiService.GetEventsAsync());
            UrlValid = "Green";
            OnPropertyChanged(nameof(AvailableEvents));
            ErrorMessage = string.Empty;
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = ex.Message;
            UrlValid = "OrangeRed";
        }

    }

    public string WebApiBaseUrl
    {
        get => _configuration.WebApiBaseUrl;
        set
        {
            if (_configuration.WebApiBaseUrl == value)
            {
                return;
            }

            _configuration.WebApiBaseUrl = value;
            OnPropertyChanged(nameof(CanCheckConnection));
            LoadEvents();
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> AvailableThemes { get; }


    public ObservableCollection<EventModel> AvailableEvents
    {
        get
        {
            if (availableEvents == null)
            {
                LoadEvents();
            }
            return availableEvents;
        }
    }


    private EventModel selectedEvent;

    public EventModel SelectedEvent
    {
        get => selectedEvent;
        set
        {
            selectedEvent = value;
            _configuration.SelectedEvent = value;
            OnPropertyChanged(nameof(SelectedEvent));
        }
    }

    public string SelectedTheme
    {
        get => _configuration.AppTheme;
        set
        {
            if (_configuration.AppTheme == value)
            {
                return;
            }

            _configuration.AppTheme = value;
            OnPropertyChanged();
        }
    }

    public string UserName
    {
        get => _configuration.UserName;
        set
        {
            if (_configuration.UserName == value)
            {
                return;
            }

            _configuration.UserName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanCheckConnection));
        }
    }

    public string Password
    {
        get => _configuration.Password;
        set
        {
            if (_configuration.Password == value)
            {
                return;
            }

            _configuration.Password = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanCheckConnection));
        }
    }

    public bool IsDebugMode
    {
        get => _configuration.IsDebugMode;
        set
        {
            if (_configuration.IsDebugMode == value)
            {
                return;
            }

            _configuration.IsDebugMode = value;
            OnPropertyChanged();
        }
    }
}