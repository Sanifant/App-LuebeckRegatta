# NuGet Package Upgrade Plan

## Overview

**Target**: Modernize the solution's Avalonia dependencies and developer-tools integration while keeping the existing .NET 10 target.
**Scope**: Five-project solution with desktop, Android, iOS, shared application, and test projects. Avalonia 12.1.4 is recommended by assessment for the resolved core packages; latest stable platform package versions were cross-checked for the platform projects.

## Tasks

### 01-update-avalonia-package-versions: Align Avalonia package versions

Update the central package versions in `src/Directory.Packages.props` for the Avalonia core and platform family to 12.1.4, DataGrid to 12.1.2, and Xaml.Behaviors.Avalonia to 12.0.7. Keep AvaloniaUI.DiagnosticsSupport at 2.2.3, which is already the latest stable version. Do not change the .NET 10 target or add package references that the projects do not currently use. The assessment found no source-breaking changes for the resolved core packages; platform and integration references not resolved by the assessment must be verified by restore/build.

**Done when**: All used Avalonia package references resolve from central package management at the planned compatible stable versions, and unrelated user edits in the working tree remain intact.

---

### 02-migrate-avalonia-developer-tools: Replace legacy diagnostics integration

Replace the obsolete `Avalonia.Diagnostics` package references in the shared application and desktop project with the already-pinned `AvaloniaUI.DiagnosticsSupport` package. Enable `AttachDeveloperTools()` from the shared `App.Initialize()` only in Debug builds, following the package guidance. This aligns desktop and mobile diagnostics support with the current Avalonia developer-tools bridge.

**Done when**: The app no longer references `Avalonia.Diagnostics`, the DiagnosticsSupport package is referenced by the shared application, and developer tools attach only in Debug builds without changing release behavior.

---

### 03-validate-avalonia-solution: Build and test the solution

Restore and build the solution's supported projects and run the test suite. Validate desktop and Android builds, and check the iOS project using the available host configuration; document any platform build that cannot be fully validated on this Windows environment. Resolve regressions caused by the package and diagnostics updates without suppressing warnings.

**Done when**: The shared, desktop, Android, and test projects build without warnings or errors, tests pass, and any iOS/macOS-only validation limitation is documented.
