# Lübeck Regatta App

[![Build and Test](https://github.com/Sanifant/App-LuebeckRegatta/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/Sanifant/App-LuebeckRegatta/actions/workflows/build-and-test.yml)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=Sanifant_App-LuebeckRegatta&token=5774c6625bcf4a4d8923d0f2a8737b5e223eed2d)](https://sonarcloud.io/summary/new_code?id=Sanifant_App-LuebeckRegatta)

Plattformübergreifende Avalonia-UI-Anwendung zur Verwaltung und Auswertung von Ruderregatten. Entwickelt für den Lübecker Regattaverein im Rahmen des OpenELP-Projekts.

## Features

- **Regatta-Übersicht** – Ereignisse, Rennen und Starterfelder auf einen Blick
- **Schiedsrichter-Dashboard** – Live-Verwaltung von Schiedsrichtern und Rennläufen inkl. Echtzeituhr und Verwarnungsfunktion
- **Öffentliche Boot- und Rennansicht** – Bootsübersichten je Rennen und Event
- **Einstellungen** – API-Basis-URL konfigurieren, Verbindung prüfen, Benutzeranmeldung, Theme-Auswahl (Light / Dark / System)
- **Server-Sent Events (SSE)** – Echtzeit-Updates vom Backend
- **Authentifizierung** – Login gegen die Regattasoftware-API

## Unterstützte Plattformen

| Plattform | Framework |
|-----------|-----------|
| Windows   | net10.0-windows |
| Linux     | net10.0 |
| Android   | net10.0-android (min. API 21) |
| iOS       | net10.0-ios (min. iOS 13) |

## Technologie-Stack

- **.NET 10** mit C# (neueste Sprachversion)
- **Avalonia UI** mit Fluent Theme und kompilierten Bindings
- **CommunityToolkit.Mvvm** – MVVM mit `[ObservableProperty]` / `[RelayCommand]`-Attributen
- **Microsoft.Extensions.DependencyInjection** – Dependency Injection
- **xUnit** – Unittests

## Projektstruktur

```
src/
├── de.openelp.regatta/        # Gemeinsame Kernbibliothek
│   ├── Interfaces/            # Service-Schnittstellen
│   ├── Models/                # DTOs und Datenmodelle
│   ├── Pages/                 # Avalonia Views (UserControls)
│   ├── Services/              # API-Services und AppConfiguration
│   └── ViewModels/            # MVVM ViewModels
├── Desktop/                   # Desktop-Projekt (Windows/Linux)
├── Android/                   # Android-Projekt
├── IOS/                       # iOS-Projekt
└── de.openelp.regatta.Tests/  # xUnit-Tests
```

## Build & Entwicklung

### Voraussetzungen

- .NET SDK 10.0.2 oder neuer
- Android Workload für Android-Builds: `dotnet workload install android`

### Desktop (Windows/Linux)

```bash
dotnet build src/Desktop/Desktop.csproj -c Release
```

### Android

```bash
dotnet build src/Android/Android.csproj -c Release
```

### iOS (nur macOS)

```bash
dotnet build src/IOS/IOS.csproj -c Release
```

### Tests ausführen

```bash
dotnet test src/de.openelp.regatta.Tests/de.openelp.regatta.Tests.csproj
```

## CI/CD

Die GitHub-Actions-Pipeline läuft bei jedem Push auf `main` und `develop` sowie bei Pull Requests:

- **Tests** – xUnit-Tests auf Ubuntu
- **Desktop-Build** – Self-Contained-Publish für Windows (`win-x64`) und Linux (`linux-x64`), inkl. MSI- bzw. DEB-Paket
- **Android-Build** – APK/AAB-Build nur bei Releases (erfordert signiertes Keystore via GitHub Secrets)
- **Release-Artefakte** – Installer werden als GitHub-Release-Assets veröffentlicht

## API-Integration

Die App kommuniziert mit der Regattasoftware-API (Standard-URL: `https://regatta-test.grinch-tech.de`):

| Service | Funktion |
|---------|----------|
| `AuthApiService` | Benutzeranmeldung |
| `EventApiService` | Ereignisse laden |
| `RaceApiService` | Rennen laden |
| `RaceHeatApiService` | Rennläufe verwalten, SSE abonnieren |
| `RefereeApiService` | Schiedsrichter laden, Verwarnungen erfassen |
| `BoatApiService` | Boote je Event und Rennen laden |

## Lizenz

Siehe [LICENSE](LICENSE).