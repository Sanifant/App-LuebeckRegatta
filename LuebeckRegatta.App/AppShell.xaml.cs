using CommunityToolkit.Mvvm.Messaging;
using LuebeckRegatta.App.Messages;
using LuebeckRegatta.App.Services;
using LuebeckRegatta.App.Views;

namespace LuebeckRegatta.App;

public partial class AppShell : Shell, IRecipient<SettingsChangedMessage>
{
    private readonly ISettingsService _settingsService;

    public AppShell()
    {
        InitializeComponent();
        
        _settingsService = new SettingsService();
        
        // Route für die Detail-Seite registrieren
        Routing.RegisterRoute(nameof(NewsDetailPage), typeof(NewsDetailPage));

        // Event für Navigation überprüfen
        Navigating += OnNavigating;

        // Registriere für Einstellungs-Änderungen
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(SettingsChangedMessage message)
    {
        // Shell UI aktualisieren wenn nötig
        // Wird automatisch durch Navigating-Event behandelt
    }

    private async void OnNavigating(object? sender, ShellNavigatingEventArgs e)
    {
        // Prüfe ob zur RaceView navigiert wird
        if (e.Target.Location.OriginalString.Contains("RaceView"))
        {
            // Prüfe ob ein Event ausgewählt ist
            if (!_settingsService.SelectedEventId.HasValue)
            {
                // Verhindere die Navigation
                e.Cancel();
                
                // Zeige Warnung an
                await DisplayAlert(
                    "Kein Event ausgewählt",
                    "Bitte wählen Sie zuerst ein Event in den Einstellungen aus.",
                    "OK");
                
                // Navigiere zu den Einstellungen
                await GoToAsync("//SettingsPage");
            }
        }
    }
}
