# 02-migrate-avalonia-developer-tools: Replace legacy diagnostics integration

Replace the obsolete `Avalonia.Diagnostics` package references in the shared application and desktop project with the already-pinned `AvaloniaUI.DiagnosticsSupport` package. Enable `AttachDeveloperTools()` from the shared `App.Initialize()` only in Debug builds, following the package guidance. This aligns desktop and mobile diagnostics support with the current Avalonia developer-tools bridge.

**Done when**: The app no longer references `Avalonia.Diagnostics`, the DiagnosticsSupport package is referenced by the shared application, and developer tools attach only in Debug builds without changing release behavior.
