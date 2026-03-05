using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using LuebeckRegatta.App.Messages;
using LuebeckRegatta.App.Services;

namespace LuebeckRegatta.App.ViewModels;

/// <summary>
/// ViewModel for the Flyout Header
/// </summary>
public class FlyoutHeaderViewModel : INotifyPropertyChanged, IRecipient<SettingsChangedMessage>
{
    private readonly ISettingsService _settingsService;
    private string _selectedEventName;

    public event PropertyChangedEventHandler? PropertyChanged;

    public FlyoutHeaderViewModel()
    {
        _settingsService = new SettingsService();
        _selectedEventName = "Kein Event ausgewählt";
        
        // Registriere für Nachrichten
        WeakReferenceMessenger.Default.Register(this);
        
        _ = LoadSelectedEventAsync();
    }

    public void Receive(SettingsChangedMessage message)
    {
        if (message.EventChanged)
        {
            _ = LoadSelectedEventAsync();
        }
    }

    public string SelectedEventName
    {
        get => _selectedEventName;
        set
        {
            if (_selectedEventName != value)
            {
                _selectedEventName = value;
                OnPropertyChanged();
            }
        }
    }

    private async Task LoadSelectedEventAsync()
    {
        try
        {
            if (_settingsService.SelectedEventId.HasValue)
            {
                var eventRepository = new Repositories.EventRepository(_settingsService);
                var eventModel = await eventRepository.GetEventAsync(_settingsService.SelectedEventId.Value);
                
                if (eventModel != null && !string.IsNullOrEmpty(eventModel.Title))
                {
                    SelectedEventName = eventModel.Title;
                }
                else
                {
                    SelectedEventName = "Kein Event ausgewählt";
                }
            }
            else
            {
                SelectedEventName = "Kein Event ausgewählt";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Laden des Events: {ex.Message}");
            SelectedEventName = "Kein Event ausgewählt";
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
