using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    /// <summary>
    /// Provides information about environment where vostok-instrumented application is hosted.
    /// </summary>
    [PublicAPI]
    internal static class EnvironmentInfo
    {
        private static Lazy<string> application = new Lazy<string>(ObtainApplicationName);
        private static Lazy<string> host = new Lazy<string>(ObtainHostname);
        private static Lazy<string> processName = new Lazy<string>(GetProcessNameOrNull);
        private static Lazy<int?> processId = new Lazy<int?>(GetProcessIdOrNull);

        /// <summary>
        /// Returns name of the application.
        /// </summary>
        public static string Application => application.Value;

        /// <summary>
        /// Returns name of machine which runs the application. 
        /// </summary>
        public static string Host => host.Value;

        /// <summary>
        /// Returns name of current process. 
        /// </summary>
        public static string ProcessName => processName.Value;

        /// <summary>
        /// Returns id of current process. 
        /// </summary>
        public static int? ProcessId => processId.Value;

        /// <summary>
        /// Returns the base directory of current assembly.
        /// </summary>
        public static string BaseDirectory => GetBaseDirectory();

        private static string ObtainApplicationName()
        {
            try
            {
                if (RuntimeDetector.IsDotNetCore)
                    return GetEntryAssemblyNameOrNull();

                return GetProcessNameOrNull() ?? GetEntryAssemblyNameOrNull();
            }
            catch
            {
                return null;
            }
        }

        private static string GetProcessNameOrNull()
        {
            try
            {
                return Process.GetCurrentProcess().ProcessName;
            }
            catch
            {
                return null;
            }
        }

        private static int? GetProcessIdOrNull()
        {
            try
            {
                return Process.GetCurrentProcess().Id;
            }
            catch
            {
                return null;
            }
        }

        private static string GetEntryAssemblyNameOrNull()
        {
            try
            {
                return Assembly.GetEntryAssembly().GetName().Name;
            }
            catch
            {
                return null;
            }
        }

        private static string GetBaseDirectory()
        {
            try
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            catch
            {
                return null;
            }
        }

        private static string ObtainHostname()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch
            {
                return "unknown";
            }
        }
    }
}