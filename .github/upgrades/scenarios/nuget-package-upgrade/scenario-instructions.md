# NuGet Package Upgrade Scenario

## Strategy
Keep the existing .NET 10 target and upgrade the Avalonia stack to the latest stable versions supported by the actual project references. Replace the legacy Avalonia.Diagnostics package with the current AvaloniaUI.DiagnosticsSupport integration.

## Preferences
- **Flow Mode**: Automatic
- **Commit Strategy**: Manual
- **Scope**: Entire solution (`src/LuebeckRegatta.slnx`)
- **Target framework**: Keep .NET 10 stable; do not move to .NET 11 Preview.
- **Package version policy**: Latest stable versions compatible with the scoped projects; no prereleases.
- **Avalonia packages**: Avalonia, Avalonia.Themes.Fluent, Avalonia.Fonts.Inter, Avalonia.Desktop, Avalonia.Android, Avalonia.iOS, and Avalonia.Browser 12.1.4; Avalonia.Controls.DataGrid 12.1.2; Xaml.Behaviors.Avalonia 12.0.7; AvaloniaUI.DiagnosticsSupport 2.2.3.
- **Assessment scan**: Full semantic scan with concrete source locations for breaking API changes.

## Decisions
- Add central package version entries at current stable versions for existing unversioned Tmds.DBus.Protocol and test package references so the solution can restore/build; do not otherwise broaden package updates beyond the approved scope.
- Do not add package references for packages not currently used by a project; update existing central version pins only where applicable.
- Package assessment found no source API breaks for Avalonia, Avalonia.Themes.Fluent, Avalonia.Fonts.Inter, and Avalonia.Desktop. It did not resolve several conditional/platform or integration packages; use the confirmed stable NuGet versions and verify the platform builds.
- Replace both Avalonia.Diagnostics references (shared app and desktop) with AvaloniaUI.DiagnosticsSupport, and attach developer tools from the shared Application only in Debug builds.
- Existing local edits are user-owned and must be preserved; edit only the lines/files required for this modernization.

## Source Control
- **Source Branch**: main
- **Working Branch**: main (user-confirmed)
- **Pending Changes**: Preserve the pre-existing local modifications; do not commit, stash, or undo them.
- **Commit Strategy**: Manual
