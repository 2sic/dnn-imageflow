# dnn-imageflow

## Imageflow image processing and optimizing http module for DNN

It use Imazen [Imageflow.NET](https://github.com/imazen/imageflow-dotnet) wrapper for [Imageflow](https://www.imageflow.io/), the image processing library for servers.

It is upgrade for very successful [dnn-imazen-imageresizer](https://github.com/2sic/dnn-imazen-imageresizer) commonly used with [2sxc](https://2sxc.org/) revolutionizing content management module on [DNN](https://www.dnnsoftware.com/). Older brother is based on classic [Imageresizer](https://imageresizing.net/) while [Imageflow](https://www.imageflow.io/) is next-generation product.

It is similar to [oqt-imageflow](https://github.com/2sic/oqtane-imageflow) for [Oqtane](https://www.oqtane.org/) and [Imageflow.NET Server](https://github.com/imazen/imageflow-dotnet-server) a super-fast image server for [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core).

## Installation

1. Please [install](https://www.nvquicksite.com/) [DNN 9.2+](https://github.com/dnnsoftware/Dnn.Platform/releases).
1. Download latest *ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip* from dnn-imageflow [releases](https://github.com/2sic/dnn-imageflow/releases) [](https://github.com/2sic/dnn-imageflow).
1. Install *ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip* dnn extension [as usually](https://www.dnnsoftware.com/docs/administrators/extensions/install-extension.html).
1. More DNN [info...](https://azing.org/dnn-community/)

### Note
1. As part of dnn extension installation it will automatically unregister older ImageResizer http module in web.config. 

## Usage

Simply store files with images in DNN website (as any other, normal, unrestricted image, eg `/Portals/0/Images/img.jpg`).

In image link use [Querystring API](https://docs.imageflow.io/querystring/introduction.html) for image manipulation, like is:
- automatically crop away whitespace
- sharpen
- fix white balance
- adjust contrast/saturation/brightness
- rotate & flip images
- crop
- resize & constrain
- produce highly optimized jpeg or webp images to reduce download times
- [more](https://docs.imageflow.io/)

### Examples

```html
<img src="img.jpg?w=50" />
<img src="img.jpg?width=100&amp;height=100&amp;mode=max&amp;scale=down" />
<img src="img.jpg?w=300&amp;h=300&amp;mode=crop&amp;scale=both" />
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

## Caching

- High performance [Imazen.HybridCache](https://www.nuget.org/packages/Imazen.HybridCache/)  (in-memory persisted database for tracking filenames with files used for bytes) is enabled by default with persistance in `App_Data\imageflow_hybrid_cache\`.

## Roadmap

- watermarks
- presets
- extensionless paths
- mapped paths
- command defaults
- ...

## Setup DEV ENV

1. Git clone this repo.
1. In VS open `ToSic.Imageflow.Dnn` solution.
1. Set **Release** configuration in configuration manager.
1. Build Solution
1. If all is OK, in `\InstallPackages` you should find DNN Library **extension packages** (to install Imageflow on other DNN installations):
	1. `ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip`
	1. `ToSic.Imageflow.Dnn_NN.NN.NN_Symbols.zip`

## References

* DotNetNuke.Web (>= 7.4.0)
  
## Included dependencies (.NETStandard 2.0)

* Imageflow.Net (0.7.24)
* Imazen.HybridCache (0.5.10)
* Imazen.Common (0.5.10)
* Imageflow.NativeRuntime.win-x86 (1.5.8-rc62)
* Imageflow.NativeRuntime.win-x86_64 (1.5.8-rc62)
* Microsoft.Extensions.Configuration.Abstractions (2.2.0)
* Microsoft.Extensions.DependencyInjection.Abstractions (2.2.0)
* Microsoft.Extensions.DependencyInjection (2.2.0)
* Microsoft.Extensions.FileProviders.Abstractions (2.2.0)
* Microsoft.Extensions.Hosting.Abstractions (2.2.0)
* Microsoft.Extensions.Logging.Abstractions (2.2.0)
* Microsoft.Extensions.Primitives (2.2.0)
* Microsoft.IO.RecyclableMemoryStream (1.2.2)
* Newtonsoft.Json (10.0.3)
* System.Buffers (4.5.1) Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
* System.Memory (4.5.4) Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
* System.Numerics.Vectors (4.4.0) Version=4.1.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
* System.Runtime.CompilerServices.Unsafe (4.5.3) Version=4.0.4.1, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a