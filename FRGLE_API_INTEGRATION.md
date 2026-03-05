# Frgle API Integration

## Übersicht
Diese Datei dokumentiert die Integration der Frgle API in die Lübeck Regatta App basierend auf der OpenAPI-Spezifikation.

## Settings-Verwaltung

### ISettingsService / SettingsService
Ein neuer Service wurde hinzugefügt, um konfigurierbare URLs und Authentifizierungsdaten zu verwalten:

**Einstellungen:**
- `NewsFeedUrl` - URL zum RSS News Feed (Standard: https://www.rudern.de/news.xml)
- `RegattaApiUrl` - Basis-URL zur Regatta API (Standard: https://frgle/api)
- `RegattaUsername` - Benutzername für die Regatta API (Standard: leer)
- `RegattaPassword` - Passwort für die Regatta API (Standard: leer)

**Verwendung:**
```csharp
var settingsService = new SettingsService();
settingsService.NewsFeedUrl = "https://custom-url.com/feed.xml";
settingsService.RegattaApiUrl = "https://custom-api.com/api";
settingsService.RegattaUsername = "username";
settingsService.RegattaPassword = "password";
```

Der SettingsService nutzt .NET MAUI Preferences, um Einstellungen persistent zu speichern.

### Settings-Seite
Eine neue Settings-Seite wurde zur App hinzugefügt:
- Verfügbar über das Flyout-Menü unter "Einstellungen"
- Ermöglicht Bearbeitung beider URLs
- Ermöglicht Eingabe von Benutzername und Passwort für die Regatta API
- Passwort-Feld mit maskierter Eingabe (IsPassword="True")
- Speichern-Button mit Bestätigungsdialog
- Zurücksetzen-Button für Standardwerte (löscht auch Authentifizierungsdaten)
- Zeigt Standardwerte zur Referenz an

## Erstellte Models

### RefereeModel
Repräsentiert einen Schiedsrichter im Regatta-System.
- **Eigenschaften**: Id, FirstName, LastName
- **Helper**: FullName (kombiniert Vor- und Nachname)

### EventModel
Repräsentiert eine Regatta-Veranstaltung.
- **Eigenschaften**: ID, Title, StartDate, EndDate, Lanes

### HeatEntryModel
Repräsentiert einen Teilnehmer (Team/Athleten) in einem Rennen.
- **Eigenschaften**: Id, Lane, ShortTeamName, TeamName, Athletes, Status, EndDate
- **Helper**: AthletesDisplay (zeigt Athleten als kommaseparierte Liste)

### RaceHeatModel
Repräsentiert einen Rennlauf mit allen Details.
- **Eigenschaften**: Id, HeatID, RaceNumber, Title, Comment, Status, PlannedStartDate, ActualStartDate, Entries

## Erstellte Repositories

### IEventRepository / EventRepository
**Endpoints:**
- `GetEventsAsync()` ? GET /api/Event
- `GetEventAsync(eventId)` ? GET /api/{eventId}/Event

**Verwendung:**
```csharp
var events = await _eventRepository.GetEventsAsync();
var singleEvent = await _eventRepository.GetEventAsync(123);
```

### IRefereeRepository / RefereeRepository
**Endpoints:**
- `GetRefereesAsync(eventId)` ? GET /api/{eventId}/Referee/
- `AddWarningAsync(eventId, refereeId, heatId)` ? PUT /api/{eventId}/Referee/{refereeId}/warning/{heatId}

**Verwendung:**
```csharp
var referees = await _refereeRepository.GetRefereesAsync(eventId);
var result = await _refereeRepository.AddWarningAsync(eventId, refereeId, heatId);
```

### IRaceHeatRepository / RaceHeatRepository
**Endpoints:**
- `GetRaceHeatsAsync(eventId)` ? GET /api/{eventId}/RaceHeat/
- `GetRaceHeatAsync(eventId, raceId)` ? GET /api/{eventId}/RaceHeat/{raceId}
- `UpdateRaceHeatAsync(eventId, raceHeat)` ? PUT /api/{eventId}/RaceHeat
- `SetRefereeAsync(eventId, raceId, referee)` ? POST /api/{eventId}/RaceHeat/{raceId}/referee
- `StopRaceAsync(eventId, raceId, raceHeat)` ? POST /api/{eventId}/RaceHeat/{raceId}/stop

**Verwendung:**
```csharp
var heats = await _raceHeatRepository.GetRaceHeatsAsync(eventId);
var heat = await _raceHeatRepository.GetRaceHeatAsync(eventId, raceId);
await _raceHeatRepository.UpdateRaceHeatAsync(eventId, heat);
await _raceHeatRepository.SetRefereeAsync(eventId, raceId, referee);
await _raceHeatRepository.StopRaceAsync(eventId, raceId, heat);
```

## Dependency Injection
Alle Services und Repositories wurden in `MauiProgram.cs` registriert:
```csharp
// Settings Service
builder.Services.AddSingleton<ISettingsService, SettingsService>();

// Repositories
builder.Services.AddSingleton<INewsRepository, NewsRepository>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
builder.Services.AddSingleton<IRefereeRepository, RefereeRepository>();
builder.Services.AddSingleton<IRaceHeatRepository, RaceHeatRepository>();

// ViewModels und Pages
builder.Services.AddTransient<SettingsViewModel>();
builder.Services.AddTransient<SettingsPage>();
```

## Architektur-Pattern
- Alle Repositories folgen dem bestehenden Pattern aus der App
- Repositories verwenden ISettingsService für konfigurierbare URLs
- Fehlerbehandlung mit try-catch und Debug.WriteLine
- Verwendung von HttpClient für API-Calls
- JSON-Serialisierung mit System.Text.Json
- PropertyNameCaseInsensitive für Deserialisierung
- CamelCase für Serialisierung
- Settings werden persistent mit .NET MAUI Preferences gespeichert

## Konfigurierbare URLs
Die URLs können nun über die Settings-Seite angepasst werden:
- **News Feed URL**: Wird vom NewsRepository verwendet
- **Regatta API URL**: Wird von allen Regatta-Repositories verwendet (EventRepository, RefereeRepository, RaceHeatRepository)

Nach dem Ändern der URLs wird empfohlen, die App neu zu starten, damit die Änderungen in allen Repositories wirksam werden.

## Authentifizierung
Benutzername und Passwort für die Regatta API werden in den Settings gespeichert:
- **Regatta Username**: Wird persistent gespeichert
- **Regatta Password**: Wird sicher mit .NET MAUI Preferences gespeichert (maskierte Eingabe)

**Hinweis für Entwickler**: Die Repositories haben aktuell noch keine HTTP Basic Authentication implementiert. Um die gespeicherten Zugangsdaten zu nutzen, können Sie den HttpClient in den Repositories wie folgt erweitern:

```csharp
// Beispiel für HTTP Basic Authentication
var username = _settingsService.RegattaUsername;
var password = _settingsService.RegattaPassword;

if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
{
    var credentials = Convert.ToBase64String(
        System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
    _httpClient.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
}
```

## Hinweise
- Das SSE (Server-Sent Events) Endpoint `/api/{eventId}/RaceHeat/subscribe` wurde nicht implementiert, da es eine spezielle Streaming-Logik erfordert
- Alle API-Methoden unterstützen CancellationToken für abbrechbare Operationen
- Fehler werden geloggt und null/leere Werte zurückgegeben
