﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

        public static string SanitizeImageExtension(string extension)
        {
            extension = extension.ToLowerInvariant().TrimStart('.');
            switch (extension)
            {
                case "png":
                    return "png";
                case "gif":
                    return "gif";
                case "webp":
                    return "webp";
                case "jpeg":
                case "jfif":
                case "jif":
                case "jfi":
                case "jpe":
                    return "jpg";
                default:
                    return null;
            }
        }

        public static string GetImageExtensionFromContentType(string contentType)
        {
            switch (contentType)
            {
                case "image/png":
                    return "png";
                case "image/gif":
                    return "gif";
                case "image/webp":
                    return "webp";
                case "image/jpeg":
                    return "jpg";
                default:
                    return null;
            }
        }

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

        internal static string Base64Hash(string data)
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

        internal static Dictionary<string, string> ToQueryDictionary(NameValueCollection requestQuery)
        {
            var dict = new Dictionary<string, string>(requestQuery.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var key in requestQuery.AllKeys)
            {
                dict.Add(key, requestQuery[key]);
            }
            return dict;
        }

        internal static string SerializeCommandString(Dictionary<string, string> finalQuery)
        {
            var qs = QueryString.Create(finalQuery.Select(p => new KeyValuePair<string, StringValues>(p.Key, p.Value)));
            return qs.ToString()?.TrimStart('?') ?? "";
        }
    }
}
