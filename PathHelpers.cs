using System;
using System.Linq;

namespace ToSic.Imageflow.Dnn
{
    internal static class PathHelpers
    {
        private static readonly string[] AcceptedImageExtensions = new string[] {
            ".png",
            ".jpg",
            ".jpeg",
            ".jfif",
            ".jif",
            ".jfi",
            ".jpe",
            ".gif",
            ".webp"
        };

        //private static string SanitizeImageExtension(string extension)
        //{
        //    extension = extension.ToLowerInvariant().TrimStart('.');
        //    switch (extension)
        //    {
        //        case "png":
        //            return "png";
        //        case "gif":
        //            return "gif";
        //        case "webp":
        //            return "webp";
        //        case "jpeg":
        //        case "jfif":
        //        case "jif":
        //        case "jfi":
        //        case "jpe":
        //            return "jpg";
        //        default:
        //            return null;
        //    }
        //}

        internal static bool IsImagePath(string path)
        {
            return AcceptedImageExtensions.Any(imageFileExtension => path.EndsWith(imageFileExtension, StringComparison.OrdinalIgnoreCase));
        }

        internal static readonly string[] SupportedQuerystringKeys = new string[]
        {
            "mode", "anchor", "flip", "sflip", "scale", "cache", "process",
            "quality", "zoom", "dpr", "crop", "cropxunits", "cropyunits",
            "w", "h", "width", "height", "maxwidth", "maxheight", "format", "thumbnail",
            "autorotate", "srotate", "rotate", "ignoreicc",
            "stretch", "webp.lossless", "webp.quality",
            "frame", "page", "subsampling", "colors", "f.sharpen", "f.sharpen_when", "down.colorspace",
            "404", "bgcolor", "paddingcolor", "bordercolor", "preset", "floatspace", "jpeg_idct_downscale_linear", "watermark",
            "s.invert", "s.sepia", "s.grayscale", "s.alpha", "s.brightness", "s.contrast", "s.saturation", "trim.threshold",
            "trim.percentpadding", "a.blur", "a.sharpen", "a.removenoise", "a.balancewhite", "dither", "jpeg.progressive",
            "encoder", "decoder", "builder", "s.roundcorners.", "paddingwidth", "paddingheight", "margin", "borderwidth", "decoder.min_precise_scaling_ratio"
        };
    }
}
