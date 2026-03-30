# Lübeck Regatta App - Codespaces Development Environment

Diese Codespace-Konfiguration richtet eine vollständige Entwicklungsumgebung für die Lübeck Regatta App ein.

## Enthaltene Features

- ✅ .NET 10.0 SDK
- ✅ Java 17
- ✅ Android Workload für Android-Builds
- ✅ Android SDK Command-line Tools, Platform Tools, API 36 und Build Tools 36.0.0
- ✅ GitHub CLI
- ✅ Docker-in-Docker (für Container-basierte Workflows)
- ✅ Node.js LTS (für zusätzliche Tools)

## VS Code Extensions

Die folgenden Extensions werden automatisch installiert:
- **C# Dev Kit** - Vollständige C#-Entwicklungsumgebung
- **C#** - Sprachunterstützung
- **.NET Test Explorer** - Testausführung und -visualisierung
- **GitHub Copilot** - AI-gestützte Code-Vervollständigung
- **GitLens** - Erweiterte Git-Integration
- **EditorConfig** - Code-Style-Konsistenz

## Verwendung

### Projekt bauen

```bash
# Android
dotnet build -f net10.0-android -c Debug

# Windows (nur auf Windows-Hosts)
dotnet build -f net10.0-windows10.0.19041.0 -c Debug

# Alle Plattformen
dotnet build
```

### Tests ausführen

```bash
dotnet test
```

### Release-Build erstellen

```bash
# Android AAB
dotnet build -f net10.0-android -c Release
```

## Hinweise

- **iOS/Mac Catalyst Builds**: Erfordern einen macOS-Host und sind in Linux-Codespaces nicht verfügbar
- **Windows Builds**: Können nur auf Windows-Hosts kompiliert werden
- **Android Builds**: Funktionieren in diesem Codespace vollständig

## Troubleshooting

Falls Probleme auftreten, können Sie das Setup manuell ausführen:

```bash
# Android SDK und Workload einrichten
bash .devcontainer/setup.sh

# Dependencies wiederherstellen
dotnet restore

# Android-Projekt neu bauen
dotnet build src/Android/Android.csproj -c Debug
```
