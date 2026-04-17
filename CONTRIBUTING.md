# Beitragen zur Lübeck Regatta App

Danke für dein Interesse, zur App beizutragen! Bitte lies diese Richtlinien, bevor du einen Pull Request erstellst.

## Voraussetzungen

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Android SDK (für Android-Builds)
- Eine laufende frgle-Instanz (für manuelle API-Tests)

## Entwicklungsumgebung einrichten

```bash
git clone https://github.com/Sanifant/App-LuebeckRegatta.git
cd App-LuebeckRegatta

# Abhängigkeiten wiederherstellen
dotnet restore src/LuebeckRegatta.slnx

# Tests ausführen
dotnet test src/de.openelp.regatta.Tests/de.openelp.regatta.Tests.csproj

# Desktop-Build
dotnet build src/Desktop/Desktop.csproj -c Debug
```

## Workflow

1. Forke das Repository und erstelle einen Feature-Branch von `main`:
   ```bash
   git checkout -b feature/mein-feature
   ```
2. Implementiere deine Änderungen (siehe Konventionen unten).
3. Stelle sicher, dass alle Tests grüner werden:
   ```bash
   dotnet test src/de.openelp.regatta.Tests/de.openelp.regatta.Tests.csproj
   ```
4. Erstelle einen Pull Request gegen den `main`-Branch.

## Code-Konventionen

- **MVVM** – Keine Logik in View-Code-behind-Dateien; alles in ViewModels.
- **CommunityToolkit.Mvvm** – `[ObservableProperty]` statt manueller `OnPropertyChanged()`-Aufrufe verwenden.
- **XML-Dokumentationskommentare** – Alle öffentlichen Klassen und Methoden mit `///`-Kommentaren versehen.
- **Nullable Reference Types** – Aktiviert projektübergreifend; `?` für nullable Typen verwenden, bei Fehlern `null` zurückgeben.
- **Fehlerbehandlung** – `try/catch` mit `Debug.WriteLine()`, kein Propagieren unbehandelter Exceptions in die UI.
- **Sprache UI** – Alle benutzerorientierten Texte auf Deutsch.
- **AXAML** – Avalonia-XAML (`.axaml`), Compiled Bindings mit `x:DataType` aktiviert lassen.

## Projektstruktur

| Ordner | Inhalt |
|--------|--------|
| `src/de.openelp.regatta/` | Gemeinsame Core-Bibliothek (ViewModels, Services, Models, Views) |
| `src/Desktop/` | Desktop-Plattform-Einstiegspunkt |
| `src/Android/` | Android-Plattform-Einstiegspunkt |
| `src/IOS/` | iOS-Plattform-Einstiegspunkt (nur macOS) |
| `src/de.openelp.regatta.Tests/` | xUnit-Tests |

## Pull Request Checkliste

- [ ] Tests laufen fehlerfrei durch
- [ ] Neue öffentliche APIs sind mit `///`-Kommentaren dokumentiert
- [ ] Keine Logik im View-Code-behind
- [ ] Keine hartkodierten Zugangsdaten oder URLs im Code
- [ ] Branch ist aktuell gegenüber `main`

## Fragen

Bei Fragen öffne einfach ein [GitHub Issue](https://github.com/Sanifant/App-LuebeckRegatta/issues).
