# dnn-imageflow

## Imageflow image processing and optimizing http module for DNN

It use Imazen [Imageflow.NET](https://github.com/imazen/imageflow-dotnet) wrapper for [Imageflow](https://www.imageflow.io/), the image processing library for servers.

It is upgrade for very successful [dnn-imazen-imageresizer](https://github.com/2sic/dnn-imazen-imageresizer) commonly used with [2sxc](https://2sxc.org/) revolutionizing content management module on [DNN](https://www.dnnsoftware.com/). Older brother is based on classic [Imageresizer](https://imageresizing.net/) while [Imageflow](https://www.imageflow.io/) is next-generation product.

It is similar to [oqt-imageflow](https://github.com/2sic/oqtane-imageflow) for [Oqtane](https://www.oqtane.org/) and [Imageflow.NET Server](https://github.com/imazen/imageflow-dotnet-server) a super-fast image server for [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core).

## Installation

1. Please [install](https://www.nvquicksite.com/) [DNN 9+](https://github.com/dnnsoftware/Dnn.Platform/releases).
1. Download latest *ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip* from dnn-imageflow [releases](https://github.com/2sic/dnn-imageflow/releases) [](https://github.com/2sic/dnn-imageflow).
1. Install *ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip* dnn extension [as usually](https://www.dnnsoftware.com/docs/administrators/extensions/install-extension.html).
1. More DNN [info...](https://azing.org/dnn-community/)

### Notes:
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

1. Ensure you have DNN in folder `Website`.
1. Clone this repo as sibling folder `ToSic.Imageflow.Dnn`.
1. In VS open `ToSic.Imageflow.Dnn` solution.
1. Restore NuGet Packages.
1. Set **Release** configuration in configuration manager.
1. Build Solution
1. If all is OK, in `Website\Install\Library` you should find DNN Library **extension packages** (to install Imageflow on other DNN installations):
	1. `ToSic.Imageflow.Dnn_NN.NN.NN_Install.zip`
	1. `ToSic.Imageflow.Dnn_NN.NN.NN_Symbols.zip`
