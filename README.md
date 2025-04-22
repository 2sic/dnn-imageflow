# DNN-Imageflow

## Overview

The **DNN-Imageflow** module provides advanced image processing and optimization for DNN websites. It leverages the Imazen [Imageflow.NET](https://github.com/imazen/imageflow-dotnet) wrapper for [Imageflow](https://www.imageflow.io/), a high-performance image processing library for servers.

This module is an upgrade to the popular [dnn-imazen-imageresizer](https://github.com/2sic/dnn-imazen-imageresizer), commonly used with the [2sxc](https://2sxc.org/) content management module for [DNN](https://www.dnnsoftware.com/). While the older version was based on [ImageResizer](https://imageresizing.net/), this module uses the next-generation [Imageflow](https://www.imageflow.io/).

It is also similar to:
- [oqt-imageflow](https://github.com/2sic/oqtane-imageflow) for [Oqtane](https://www.oqtane.org/)
- [Imageflow.NET Server](https://github.com/imazen/imageflow-dotnet-server), a high-performance image server for [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core).

---

## Installation

1. Install [DNN 9.11.0+](https://github.com/dnnsoftware/Dnn.Platform/releases) using [nvQuickSite](https://www.nvquicksite.com/).
2. Download the latest `ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip` from the [releases page](https://github.com/2sic/dnn-imageflow/releases).
3. Install the extension using the [DNN extension installation guide](https://www.dnnsoftware.com/docs/administrators/extensions/install-extension.html).
4. For more DNN-related information, visit the [DNN Community](https://azing.org/dnn-community/).

### Note
- During installation, the module will automatically unregister the older `ImageResizer` HTTP module in `web.config`.

---

## Usage

Store image files in your DNN website (e.g., `/Portals/0/Images/img.jpg`). Use the [Querystring API](https://docs.imageflow.io/querystring/introduction.html) to manipulate images dynamically.

### Features
- Automatically crop whitespace
- Sharpen images
- Adjust white balance, contrast, saturation, and brightness
- Rotate and flip images
- Crop and resize images
- Generate highly optimized JPEG or WebP images
- [More features...](https://docs.imageflow.io/)

### Examples

```html
<img src="img.jpg?w=50" />
<img src="img.jpg?width=100&height=100&mode=max&scale=down" /> 
<img src="img.jpg?w=300&h=300&mode=crop&scale=both" /> 
<img src="img.jpg?format=webp" /> 
<img src="img.jpg?s.grayscale=true" />
<img src="img.jpg?s.grayscale=ry" />
<img src="img.jpg?s.grayscale=bt709" />
<img src="img.jpg?s.grayscale=flat" />
<img src="img.jpg?s.sepia=true" />
<img src="img.jpg?s.invert=true" />
<img src="img.jpg?s.alpha=0.25" />
<img src="img.jpg?s.contrast=-0.80" />
<img src="img.jpg?s.brightness=0.5" />
<img src="img.jpg?s.saturation=-0.5" />
```

---

## Caching

The module uses the high-performance [Imazen.HybridCache](https://www.nuget.org/packages/Imazen.HybridCache/) by default. This in-memory persisted database tracks filenames and file bytes. Cache data is stored in `App_Data\imageflow_hybrid_cache\`.

---

## Roadmap

Planned features include:
- Watermarks
- Presets
- Extensionless paths
- Mapped paths
- Command defaults
- And more...

---

## Development Setup

1. Clone this repository.
2. Open the `ToSic.Imageflow.Dnn` solution in Visual Studio.
3. Set the **Release** configuration in the Configuration Manager.
4. Build the solution.
5. If successful, the following extension packages will be available in the `\InstallPackages` folder:
   - `ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip`
   - `ToSic.Imageflow.Dnn_NN.NN.NN_Symbols.zip`

---

## References

- [DotNetNuke.Web (>= 9.11.0)](https://www.nuget.org/packages/DotNetNuke.Web)
- [Imageflow.NativeRuntime.win-x86_64](https://www.nuget.org/packages/Imageflow.NativeRuntime.win-x86_64) ([2.1.0-rc11](https://www.nuget.org/packages/Imageflow.NativeRuntime.win-x86_64/2.1.0-rc11))
- [Imageflow.Net](https://www.nuget.org/packages/Imageflow.Net) ([0.14.0-rc01](https://www.nuget.org/packages/Imageflow.Net/0.14.0-rc01))
- [Imazen.HybridCache](https://www.nuget.org/packages/Imazen.HybridCache) ([0.8.3](https://www.nuget.org/packages/Imazen.HybridCache/0.8.3))

---

## Dependencies

- [DNN Imageflow v1.12.0](Docs/dependecies-v1.12.0.md)
- [DNN Core Shared Dependencies by Version](Docs/dnn-shared-dependecies.md)

---

## Versioning

When updating the version, ensure the following files are updated:
- `Properties/AssemblyInfo.cs` (update in 2 places)
- `ToSic.Imageflow.Dnn.dnn` (update the package version)
- `ToSic.Imageflow.Dnn_Symbols.dnn` (update in 2 places: package and dependency)
- `releasenotes.txt` (update the version in 1 place)
