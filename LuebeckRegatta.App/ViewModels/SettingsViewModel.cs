using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using LuebeckRegatta.App.Messages;
using LuebeckRegatta.App.Models;
using LuebeckRegatta.App.Repositories;
using LuebeckRegatta.App.Services;

namespace LuebeckRegatta.App.ViewModels;

/// <summary>
/// ViewModel for the Settings page
/// </summary>
public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;
    private readonly IEventRepository _eventRepository;
    private string _newsFeedUrl;
    private string _regattaApiUrl;
    private string _regattaUsername;
    private string _regattaPassword;
    private EventModel? _selectedEvent;
    private bool _isLoadingEvents;

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsViewModel()
    {
        _settingsService = new SettingsService();
        _eventRepository = new EventRepository(_settingsService);
        _newsFeedUrl = _settingsService.NewsFeedUrl;
        _regattaApiUrl = _settingsService.RegattaApiUrl;
        _regattaUsername = _settingsService.RegattaUsername;
        _regattaPassword = _settingsService.RegattaPassword;

        SaveCommand = new Command(ExecuteSave);
        ResetCommand = new Command(ExecuteReset);
        LoadEventsCommand = new Command(async () => await LoadEventsAsync());

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await LoadEventsAsync();
        
        if (_settingsService.SelectedEventId.HasValue)
        {
            SelectedEvent = Events.FirstOrDefault(e => e.ID == _settingsService.SelectedEventId.Value);
        }
    }

    public ObservableCollection<EventModel> Events { get; } = new();

    public string NewsFeedUrl
    {
        get => _newsFeedUrl;
        set
        {
            if (_newsFeedUrl != value)
            {
                _newsFeedUrl = value;
                OnPropertyChanged();
            }
        }
    }

    public string RegattaApiUrl
    {
        get => _regattaApiUrl;
        set
        {
            if (_regattaApiUrl != value)
            {
                _regattaApiUrl = value;
                OnPropertyChanged();
            }
        }
    }

    public string RegattaUsername
    {
        get => _regattaUsername;
        set
        {
            if (_regattaUsername != value)
            {
                _regattaUsername = value;
                OnPropertyChanged();
            }
        }
    }

    public string RegattaPassword
    {
        get => _regattaPassword;
        set
        {
            if (_regattaPassword != value)
            {
                _regattaPassword = value;
                OnPropertyChanged();
            }
        }
    }

    public EventModel? SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            if (_selectedEvent != value)
            {
                _selectedEvent = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsLoadingEvents
    {
        get => _isLoadingEvents;
        set
        {
            if (_isLoadingEvents != value)
            {
                _isLoadingEvents = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand LoadEventsCommand { get; }

    private async Task LoadEventsAsync()
    {
        IsLoadingEvents = true;
        try
        {
            var events = await _eventRepository.GetEventsAsync();
            if (events != null)
            {
                Events.Clear();
                foreach (var ev in events)
                {
                    Events.Add(ev);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Laden der Events: {ex.Message}");
        }
        finally
        {
            IsLoadingEvents = false;
        }
    }

    private async void ExecuteSave()
    {
        var previousEventId = _settingsService.SelectedEventId;
        
        _settingsService.NewsFeedUrl = NewsFeedUrl;
        _settingsService.RegattaApiUrl = RegattaApiUrl;
        _settingsService.RegattaUsername = RegattaUsername;
        _settingsService.RegattaPassword = RegattaPassword;
        _settingsService.SelectedEventId = SelectedEvent?.ID;

        // Sende Nachricht dass Einstellungen geändert wurden
        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage 
        { 
            EventChanged = previousEventId != _settingsService.SelectedEventId 
        });

        await Application.Current.MainPage.DisplayAlert(
            "Einstellungen gespeichert",
            "Die Einstellungen wurden erfolgreich gespeichert und übernommen.",
            "OK");
    }

    private async void ExecuteReset()
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Zurücksetzen",
            "Möchten Sie alle Einstellungen auf die Standardwerte zurücksetzen?",
            "Ja",
            "Nein");

        if (confirm)
        {
            _settingsService.ResetToDefaults();
            NewsFeedUrl = _settingsService.NewsFeedUrl;
            RegattaApiUrl = _settingsService.RegattaApiUrl;
            RegattaUsername = _settingsService.RegattaUsername;
            RegattaPassword = _settingsService.RegattaPassword;
            SelectedEvent = null;

            await Application.Current.MainPage.DisplayAlert(
                "Zurückgesetzt",
                "Die Einstellungen wurden auf die Standardwerte zurückgesetzt.",
                "OK");
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
