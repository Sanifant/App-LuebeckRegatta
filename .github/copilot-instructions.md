# Lübeck Regatta App - AI Coding Agent Instructions

## Project Overview
Cross-platform .NET MAUI mobile app for rowing news from rudern.de. Targets Android, iOS, Mac Catalyst, and Windows using .NET 10.0.

## Architecture Patterns

### MVVM Without CommunityToolkit.Mvvm Attributes
- ViewModels use manual `INotifyPropertyChanged` implementation, NOT `[ObservableProperty]` attributes
- Example: See [MainPageViewModel.cs](../LuebeckRegatta.App/ViewModels/MainPageViewModel.cs) - implements `INotifyPropertyChanged` with explicit property setters calling `OnPropertyChanged()`
- Commands use `new Command(async () => await Method())` instead of `[RelayCommand]`
- Despite having `CommunityToolkit.Mvvm` package, the codebase uses traditional MVVM pattern

### Dependency Injection Setup
- Services registered in [MauiProgram.cs](../LuebeckRegatta.App/MauiProgram.cs): repositories as Singleton, ViewModels/Pages as Transient
- ViewModels initialize repositories directly in constructors: `_newsRepository = new NewsRepository()`
- Repository interface pattern: `INewsRepository` → `NewsRepository` implementation

### Shell Navigation
- [AppShell.xaml](../LuebeckRegatta.App/AppShell.xaml) defines flyout menu with custom header/footer controls
- Navigation uses `await Shell.Current.GoToAsync(nameof(NewsDetailPage), parameters)` with Dictionary parameters
- Route names match class names (e.g., "MainPage", "NewsDetailPage")

### RSS Feed Integration
- `NewsRepository` fetches XML from `https://www.rudern.de/news.xml` using HttpClient + XDocument parsing
- HTML stripping: `Regex.Replace(html, "<.*?>", string.Empty)` for clean descriptions
- Image extraction from description HTML via regex: `src="([^"]+)"`

## Build & Development

### Platform-Specific Builds
```bash
# Android (Release with signing)
dotnet build -f net10.0-android -c Release

# iOS (with IPA generation)
dotnet build -f net10.0-ios -c Release /p:BuildIpa=True

# Windows
dotnet build -f net10.0-windows10.0.19041.0 -c Release
```

### Android Release Configuration
- Uses keystore signing (configured in [LuebeckRegatta.App.csproj](../LuebeckRegatta.App/LuebeckRegatta.App.csproj))
- Password properties in [Directory.Build.props](../LuebeckRegatta.App/Directory.Build.props)
- Output format: AAB (`AndroidPackageFormat=aab`)

### Testing
- xUnit test project: [LuebeckRegatta.App.Tests](../LuebeckRegatta.App.Tests/)
- Run tests: `dotnet test`
- Sample integration test in [NewsRepositoryTests.cs](../LuebeckRegatta.App/Tests/NewsRepositoryTests.cs) - tests live RSS feed

## CI/CD Pipeline
- GitHub Actions workflow: [build-and-test.yml](../.github/workflows/build-and-test.yml)
- Multi-platform builds on Windows (Android/Windows) and macOS (iOS)
- Artifacts uploaded with 7-day retention

## Key Conventions
1. **No XAML code-behind logic** - ViewModels handle all business logic
2. **German UI text** - App displays in German (e.g., "Rennen", "Lübeck Regatta")
3. **ObservableCollection pattern** - Use `NewsItems.Clear()` + `Add()` loop for refresh
4. **Error handling** - Try-catch with Debug.WriteLine, return null on failures
5. **Async initialization** - ViewModels call async methods in constructor with `_ = LoadNewsAsync()`
6. **XML Documentation Comments** - Always add /// documentation comments for classes, methods, and public members
   ```csharp
   /// <summary>
   /// Loads news items from the RSS feed
   /// </summary>
   public async Task LoadNewsAsync() { ... }
   ```
