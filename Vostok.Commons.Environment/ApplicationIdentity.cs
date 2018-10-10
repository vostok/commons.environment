using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    // CR(iloktionov): 1. Rename to EnvironmentInfo
    // CR(iloktionov): 2. Add local hostname provider
    // CR(iloktionov): 3. Use it instead of https://github.com/vostok/tracing/blob/master/Vostok.Tracing/Helpers/EnvironmentHelper.cs
    /// <summary>
    /// Provides application name to identify different applications which use vostok libraries.
    /// </summary>
    [PublicAPI]
    internal static class EnvironmentInfo
    {
        private static Lazy<string> application = new Lazy<string>(ObtainApplicationName);
        private static Lazy<string> host = new Lazy<string>(ObtainHostname);

        public static string Application => application.Value;
        public static string Host => host.Value;

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