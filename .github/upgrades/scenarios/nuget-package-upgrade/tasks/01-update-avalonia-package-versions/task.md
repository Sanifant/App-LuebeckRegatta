# 01-update-avalonia-package-versions: Align Avalonia package versions

Update the central package versions in `src/Directory.Packages.props` for the Avalonia core and platform family to 12.1.4, DataGrid to 12.1.2, and Xaml.Behaviors.Avalonia to 12.0.7. Keep AvaloniaUI.DiagnosticsSupport at 2.2.3, which is already the latest stable version. Do not change the .NET 10 target or add package references that the projects do not currently use. The assessment found no source-breaking changes for the resolved core packages; platform and integration references not resolved by the assessment must be verified by restore/build.

**Done when**: All used Avalonia package references resolve from central package management at the planned compatible stable versions, and unrelated user edits in the working tree remain intact.
