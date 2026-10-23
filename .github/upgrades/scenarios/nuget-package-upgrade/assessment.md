# NuGet package upgrade assessment

_Mode: **full scan** — exact breaking-change call sites were located across all scoped projects._

## Recommended versions

- **Avalonia**: **12.1.4** (unified across 1 project(s)).
- **Avalonia.Android**: not referenced by any scoped project, or no supported version was found.
- **Avalonia.Browser**: not referenced by any scoped project, or no supported version was found.
- **Avalonia.Controls.DataGrid**: not referenced by any scoped project, or no supported version was found.
- **Avalonia.Desktop**: **12.1.4** (unified across 1 project(s)).
- **Avalonia.Diagnostics**: not referenced by any scoped project, or no supported version was found.
- **Avalonia.Fonts.Inter**: **12.1.4** (unified across 1 project(s)).
- **Avalonia.iOS**: not referenced by any scoped project, or no supported version was found.
- **Avalonia.Themes.Fluent**: **12.1.4** (unified across 1 project(s)).
- **AvaloniaUI.DiagnosticsSupport**: not referenced by any scoped project, or no supported version was found.
- **Xaml.Behaviors.Avalonia**: not referenced by any scoped project, or no supported version was found.

## Public API changes

> **Types moved (namespace changed) are not removals.** A moved type keeps its name and members;
> the fix is a `using`-directive change, not a rewrite. Do not treat a moved type as deleted.

- **Avalonia**: no source-breaking public API changes detected — see [`apidiff/Avalonia.apidiff.md`](apidiff/Avalonia.apidiff.md).
- **Avalonia.Themes.Fluent**: no source-breaking public API changes detected — see [`apidiff/Avalonia.Themes.Fluent.apidiff.md`](apidiff/Avalonia.Themes.Fluent.apidiff.md).
- **Avalonia.Fonts.Inter**: no source-breaking public API changes detected — see [`apidiff/Avalonia.Fonts.Inter.apidiff.md`](apidiff/Avalonia.Fonts.Inter.apidiff.md).
- **Avalonia.Desktop**: no source-breaking public API changes detected — see [`apidiff/Avalonia.Desktop.apidiff.md`](apidiff/Avalonia.Desktop.apidiff.md).

## Breaking-change findings

- Usages of removed API (PkgApi.0001): 0
- Usages of signature-changed API (PkgApi.0002): 0
- Version divergence findings (Pkg.0003): 0
- Requested-version-unsupported findings (Pkg.0002): 0

Exact call-site locations are available via `query_dotnet_assessment` (list issues per project/file).

## Next steps

1. Proceed to planning to triage the API changes above and plan the code fixes (if any).

