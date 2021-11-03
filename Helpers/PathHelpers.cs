using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ToSic.Imageflow.Dnn.Helpers
{
    internal static class PathHelpers
    {
        private static readonly string[] AcceptedImageExtensions = {
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

        public static bool IsImagePath(string path)
        {
            return AcceptedImageExtensions.Any(imageFileExtension => path.EndsWith(imageFileExtension, StringComparison.OrdinalIgnoreCase));
        }

        public static readonly string[] SupportedQuerystringKeys = {
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

        public static string Base64Hash(string data)
        {
            using (var sha2 = SHA256.Create())
            {
                var stringBytes = Encoding.UTF8.GetBytes(data);
                // check cache and return if cached
                var hashBytes =
                    sha2.ComputeHash(stringBytes);
                return Convert.ToBase64String(hashBytes)
                    .Replace("=", string.Empty)
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }

        public static Dictionary<string, string> ToQueryDictionary(NameValueCollection requestQuery)
        {
            var dict = new Dictionary<string, string>(requestQuery.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var key in requestQuery.AllKeys)
            {
                dict.Add(key, requestQuery[key]);
            }
            return dict;
        }

        public static string SerializeCommandString(Dictionary<string, string> finalQuery)
        {
            var qs = QueryString.Create(finalQuery.Select(p => new KeyValuePair<string, StringValues>(p.Key, p.Value)));
            return qs.ToString()?.TrimStart('?');
        }
    }
}
