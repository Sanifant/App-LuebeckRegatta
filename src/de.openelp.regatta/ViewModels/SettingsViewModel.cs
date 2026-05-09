using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace de.openelp.regatta.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly IAppConfiguration _configuration;
    private readonly ObservableCollection<EventModel> _availableEvents = new();

    public SettingsViewModel(IAppConfiguration? configuration = null)
    {
        _configuration = configuration ?? AppConfiguration.Current;
        this.AvailableThemes = new ObservableCollection<string>() { "Light", "Dark", "System" };

        if (!String.IsNullOrWhiteSpace(_configuration.WebApiBaseUrl)) LoadEvents();

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
        var user = this._configuration.User;
        if (user == null)
        {
            this.CanSaveChanges = false;
            UrlValid = "OrangeRed";
            ErrorMessage = "Benutzer konnte nicht geladen werden.";
        }
        else
        {
            this.CanSaveChanges = true;

            this._availableEvents.Clear();
            foreach (var eventModel in user.Events ?? Enumerable.Empty<EventModel>())
            {
                this._availableEvents.Add(eventModel);
            }

            OnPropertyChanged(nameof(AvailableEvents));
            OnPropertyChanged(nameof(CanSaveChanges));
            UrlValid = "Green";
            ErrorMessage = string.Empty;
        }
    }

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public bool CanSaveChanges { get; set; }

    [ObservableProperty]
    private string _urlValid = string.Empty;


    [RelayCommand]
    public async Task SaveChanges()
    {
        var persistence = App.Services.GetService<IConfigurationPersistence>();
        if (persistence != null)
        {
            await persistence.SaveAsync(_configuration).ConfigureAwait(false);
        }
    }

    [RelayCommand]
    public async Task ParseUrl()
    {
        if (WebApiBaseUrl != string.Empty)
        {
            var uri = new Uri(WebApiBaseUrl);
            var eventName = uri.Segments[1].TrimEnd('/');

            var eventApiService = new EventApiService(this._configuration);
            try
            {
                var selectedEvent = await eventApiService.GetEventByNameAsync(eventName);
                if (selectedEvent != null)
                {
                    SelectedEvent = selectedEvent;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                this.CanSaveChanges = false;
                UrlValid = "OrangeRed";
            }
        }


    }

    private async void LoadEvents()
    {
        var eventApiService = new EventApiService(this._configuration);
        try
        {
                this._availableEvents.Clear();
                var events = await eventApiService.GetEventsAsync();
                foreach (var eventModel in events ?? Enumerable.Empty<EventModel>())
                {
                    this._availableEvents.Add(eventModel);
                }
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
        get => _availableEvents;
    }


    private EventModel _selectedEvent = new();

    public EventModel SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            _selectedEvent = value;
            _configuration.SelectedEvent = value;
            OnPropertyChanged();
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