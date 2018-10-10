using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    /// <summary>
    /// Determines the runtime on which the application is running.
    /// </summary>
    [PublicAPI]
    internal static class RuntimeDetector
    {
        /// <summary>
        /// Returns <c>true</c> when the application is running on Mono
        /// </summary>
        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core
        /// </summary>
        public static bool IsDotNetCore { get; } = HasCoreLib();
        
        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Framework
        /// </summary>
        public static bool IsDotNetFramework { get; } = RuntimeEnvironment.GetRuntimeDirectory().Contains(@"Microsoft.NET\Framework");

        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core 2.0
        /// </summary>
        public static bool IsDotNetCore20 { get; } = IsDotNetCore && !HasSocketsHttpHandler();

        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core 2.1.0 or newer
        /// </summary>
        public static bool IsDotNetCore21AndNewer { get; } = IsDotNetCore && HasSocketsHttpHandler();

        private static bool HasSocketsHttpHandler()
        {
            try
            {
                return typeof(HttpClient).Assembly.GetTypes().Any(x => string.Equals(x.FullName, "System.Net.Http.SocketsHttpHandler", StringComparison.Ordinal));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool HasCoreLib()
        {
            try
            {
                return string.Equals(typeof(Stream).Assembly.GetName().Name, "System.Private.CoreLib", StringComparison.Ordinal);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}