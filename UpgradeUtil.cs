using System.IO;
using System.Web;

namespace ToSic.Imageflow.Dnn
{
    internal static class UpgradeUtil
    {
        public static bool Upgraded { get; private set; }
        private const string PendingExtension = "pending";

        // Replace native assemblies
        private static readonly string[] NativeAssemblies = {
            @"win-arm64\native\imageflow.dll",
            @"win-x64\native\imageflow.dll",
            @"win-x86\native\imageflow.dll"
        };

        private static readonly object UpgradeLock = new object();
        private static string Bin { get; } = HttpContext.Current.Server.MapPath(@"~\bin\");
        private static string Runtimes { get; } = Path.Combine(Bin, "runtimes"); // Dnn install should place native assemblies in this folder

        /// <summary>
        /// Ensure that native assemblies are in place, before we start to use them.
        /// </summary>
        public static void UpgradeNativeAssemblies()
        {
            // ensure that upgrade is executed only ounce
            if (Upgraded) return;

            lock (UpgradeLock)
            {
                // after waiting...Any pending native assemblies?
                if (!Upgraded && Directory.Exists(Runtimes))
                    Upgraded = Directory.GetFiles(Runtimes, $"*.{PendingExtension}", SearchOption.AllDirectories).Length == 0;

                // if upgrade is already done before and there is no pending assemblies, than we are done
                if (Upgraded) return;

                // this part is tricky...
                ReplaceNativeAssemblies();
            }
        }

        /// <summary>
        /// Replacing of native assemblies is only possible when this assemblies are not already locked,
        /// because are in use and loaded.
        /// This part is tricky and it is possible that it will be executed more times, until all work is done.
        /// </summary>
        private static void ReplaceNativeAssemblies()
        {
            var withoutExceptions = true;

            foreach (var file in NativeAssemblies)
            {
                var sourceFile = Path.Combine(Runtimes, $"{file}.{PendingExtension}");
                if (!File.Exists(sourceFile)) continue;

                try
                {
                    var destinationFile = Path.Combine(Runtimes, file);
                    if (File.Exists(destinationFile)) File.Delete(destinationFile);
                    // Rename file, remove ".pending" extension to get normal "assembly.dll" filename
                    File.Move(sourceFile, destinationFile);
                }
                catch
                {
                    withoutExceptions = false;
                }
            }

            Upgraded = withoutExceptions;
        }
    }
}
