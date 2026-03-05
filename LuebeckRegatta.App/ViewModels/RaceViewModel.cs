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
/// ViewModel for the Race view
/// </summary>
public class RaceViewModel : INotifyPropertyChanged, IRecipient<SettingsChangedMessage>
{
    private readonly IRaceHeatRepository _raceHeatRepository;
    private readonly ISettingsService _settingsService;
    private bool _isLoading;
    private bool _isRefreshing;
    private string _eventTitle;

    public event PropertyChangedEventHandler? PropertyChanged;

    public RaceViewModel()
    {
        _settingsService = new SettingsService();
        _raceHeatRepository = new RaceHeatRepository(_settingsService);
        _eventTitle = "Rennen";

        RefreshCommand = new Command(async () => await RefreshAsync());
        
        // Registriere für Einstellungs-Änderungen
        WeakReferenceMessenger.Default.Register(this);
        
        _ = LoadRaceHeatsAsync();
    }

    public ObservableCollection<RaceHeatModel> RaceHeats { get; } = new();

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            if (_isRefreshing != value)
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
    }

    public string EventTitle
    {
        get => _eventTitle;
        set
        {
            if (_eventTitle != value)
            {
                _eventTitle = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand RefreshCommand { get; }

    public void Receive(SettingsChangedMessage message)
    {
        if (message.EventChanged)
        {
            _ = LoadRaceHeatsAsync();
        }
    }

    private async Task LoadRaceHeatsAsync()
    {
        IsLoading = true;
        try
        {
            if (!_settingsService.SelectedEventId.HasValue)
            {
                RaceHeats.Clear();
                EventTitle = "Kein Event ausgewählt";
                return;
            }

            var eventId = _settingsService.SelectedEventId.Value;
            
            // Lade Event-Details für den Titel
            var eventRepository = new EventRepository(_settingsService);
            var eventModel = await eventRepository.GetEventAsync(eventId);
            EventTitle = eventModel?.Title ?? "Rennen";

            // Lade Rennen
            var raceHeats = await _raceHeatRepository.GetRaceHeatsAsync(eventId);
            if (raceHeats != null)
            {
                RaceHeats.Clear();
                foreach (var raceHeat in raceHeats.OrderBy(r => r.PlannedStartDate).ThenBy(r => r.RaceNumber))
                {
                    RaceHeats.Add(raceHeat);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Laden der Rennen: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadRaceHeatsAsync();
        IsRefreshing = false;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
