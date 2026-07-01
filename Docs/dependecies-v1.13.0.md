# DNN Imageflow compatibility for DNN 10.2.2

This document captures the packaging and runtime layout that the current Imageflow work is preparing for the 1.13.0 release.

## Target baseline

- DNN minimum/core dependency: 10.2.2
- Project target framework: .NET Framework 4.8
- Compatibility line kept for shared dependencies: Microsoft.Extensions.DependencyInjection 8.0.0 and Microsoft.Bcl.AsyncInterfaces 8.0.0

## Installation layout

The module installs the shared Imageflow runtime assemblies directly into the DNN `bin` folder. This keeps the layout simpler and aligns with the current expectation that the latest supported dependency set should live alongside the module assembly in the shared runtime folder.

### Packaged managed assemblies

- ToSic.Imageflow.Dnn.dll
- Imageflow.Net.dll
- Imazen.Common.dll
- Imazen.HybridCache.dll
- Microsoft.Extensions.Configuration.Abstractions.dll (2.2.0.18315)
- Microsoft.Extensions.FileProviders.Abstractions.dll (2.2.0.18315)
- Microsoft.Extensions.Hosting.Abstractions.dll (2.2.0.18316)
- Microsoft.Extensions.Logging.Abstractions.dll (2.2.0.18315)
- Microsoft.Extensions.Primitives.dll (2.2.0.18315)
- Microsoft.IO.RecyclableMemoryStream.dll (3.0.1.0)

The DNN-shipped `System.*`, `Microsoft.Bcl.AsyncInterfaces`, and `Microsoft.Extensions.DependencyInjection*` assemblies are intentionally omitted from the package.

## Why this layout exists

This preserves compatibility with DNN 10.2.2+ by relying on the DNN-provided shared runtime assemblies and only shipping the Imageflow-specific managed dependencies in the main `bin` folder. The manifest removes stale `codeBase` entries left by older `bin/Imageflow` installs and lets DNN's Assembly component manage binding redirects.

## Packaging notes

The DNN manifest and the package build targets are aligned with this structure:

- `ToSic.Imageflow.Dnn.dnn` declares one `Assembly` component with `<basePath>bin</basePath>`
- `Build/LibraryPackage.targets` copies packaged managed assemblies into `Package/bin`
- The resource zip is only used for the native `win-x64` runtime payload
