# Lübeck Regatta App - AI Coding Agent Instructions

## Project Overview
Cross-platform Avalonia UI application for regatta management and referee coordination. 
Targets Android, iOS, Desktop (Windows/Linux), and Mac using .NET 10.0 and integrates with the frgle API for race heat and referee data.

## Architecture Patterns

### MVVM with CommunityToolkit.Mvvm Attributes
- ViewModels use `[ObservableProperty]` attributes from CommunityToolkit.Mvvm
- Example: See [MainViewModel.cs](../src/de.openelp.regatta/ViewModels/MainViewModel.cs) - uses `[ObservableProperty]` for auto-generating properties with `INotifyPropertyChanged`
- Base class pattern: All ViewModels inherit from `ViewModelBase : ObservableObject`
- No manual `OnPropertyChanged()` calls needed - handled by source generators

### Dependency Injection & Service Layer
- Services registered in DI container (likely in App.axaml.cs or Program.cs)
- Service interface pattern: `IFrgleApiService` → `FrgleApiService` implementation
- HttpClient injected into services for API communication
- Services in [Services/](../src/de.openelp.regatta/Services/) folder

### Avalonia UI Framework
- XAML files use `.axaml` extension (Avalonia XAML)
- MainWindow hosts Views: [MainWindow.axaml](../src/de.openelp.regatta/Views/MainWindow.axaml) → [MainView.axaml](../src/de.openelp.regatta/Views/MainView.axaml)
- ViewLocator pattern for automatic View-ViewModel binding
- Fluent theme: `<FluentTheme />` in App.axaml
- Compiled bindings enabled: `<AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>`
- DataContext binding syntax: `x:DataType="vm:MainViewModel"` for design-time support

### frgle API Integration
- `FrgleApiService` communicates with frgle API (default base URL: `https://frgle`)
- API endpoints:
  - `GET /frgle/api/{eventId}/Referee` - Get all referees
  - `PUT /frgle/api/0/Referee/{refereeId}/warning/{heatId}` - Add warning
  - `PUT /frgle/api` - Update race heat (JSON body)
  - `GET /frgle/api/{eventId}/RaceHeat` - Get race heats
- Uses `HttpClient.GetFromJsonAsync<T>()` and `PutAsJsonAsync()` for JSON serialization
- Error handling: Try-catch with `Debug.WriteLine()`, returns null on failures

## Build & Development

### Platform-Specific Builds
```bash
# Desktop (Windows/Linux/Mac)
dotnet build src/Desktop/Desktop.csproj -c Release

# Android
dotnet build src/Android/Android.csproj -c Release

# iOS (macOS only)
dotnet build src/IOS/IOS.csproj -c Release
```

### Android Configuration
- Target framework: `net10.0-android`
- Minimum Android version: API 21 (Android 5.0)
- Package format: APK (`<AndroidPackageFormat>apk</AndroidPackageFormat>`)
- Application ID: `com.CompanyName.AvaloniaApplication1`

### iOS Configuration
- Target framework: `net10.0-ios` (conditionally on macOS)
- Minimum iOS version: 13.0

### Testing
- xUnit test project: [de.openelp.regatta.Tests](../src/de.openelp.regatta.Tests/)
- Run tests: `dotnet test src/de.openelp.regatta.Tests/de.openelp.regatta.Tests.csproj`
- Sample test in [MainViewModelTests.cs](../src/de.openelp.regatta.Tests/ViewModels/MainViewModelTests.cs) - tests property changed notifications

## CI/CD Pipeline
- GitHub Actions workflow: [build-and-test.yml](../.github/workflows/build-and-test.yml)
- Test job runs on Ubuntu with .NET 10.0
- Multi-platform builds: Desktop (Ubuntu/Windows), Android (Ubuntu)
- Release builds triggered on GitHub release creation
- Artifacts uploaded with 7-day retention
- Desktop release assets automatically attached to GitHub releases as ZIP files

## Key Conventions
1. **XML Documentation Comments** - Always add /// documentation comments for classes, methods, and public members
   ```csharp
   /// <summary>
   /// Gets all race heats for an event
   /// </summary>
   /// <param name="eventId">The event ID</param>
   public async Task<List<RaceHeatModel>?> GetRaceHeatsAsync(int eventId) { ... }
   ```

2. **Nullable Reference Types** - Enabled project-wide (`<Nullable>enable</Nullable>`)
   - Use `?` suffix for nullable types (e.g., `string?`, `int?`, `List<T>?`)
   - Return null from service methods on errors

3. **Models in Models folder** - DTOs/entities in [Models/](../src/de.openelp.regatta/Models/)
   - Example: `RefereeModel`, `RaceHeatModel`, `HeatEntryModel`
   - Simple POCOs with nullable properties

4. **German UI text** - App displays in German (e.g., "Lübeck Regatta")

5. **Error handling pattern**:
   ```csharp
   try
   {
       var response = await _httpClient.GetAsync($"{_baseUrl}/api/endpoint");
       response.EnsureSuccessStatusCode();
       return await response.Content.ReadFromJsonAsync<Model>();
   }
   catch (Exception ex)
   {
       Debug.WriteLine($"Error description: {ex.Message}");
       return null;
   }
   ```

6. **No code-behind logic in Views** - Keep Views simple, all logic in ViewModels

7. **Avalonia-specific patterns**:
   - Use `UserControl` for reusable views
   - `Design.DataContext` for design-time previews
   - Compiled bindings with `x:DataType` for performance

8. **Project structure**:
   - Core library: `de.openelp.regatta` (shared code)
   - Platform heads: `Desktop`, `Android`, `IOS` (reference core project)
   - Tests: `de.openelp.regatta.Tests`
