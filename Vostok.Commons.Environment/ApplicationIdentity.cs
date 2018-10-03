using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    /// <summary>
    /// Provides application name to identify different applications which use vostok libraries.
    /// </summary>
    [PublicAPI]
    internal static class ApplicationIdentity
    {
        private static Lazy<string> identity = new Lazy<string>();

        public static string Get()
        {
            return identity.Value;
        }

        /// <returns>The name of application that use vostok library.</returns>
        private static string GetIdentity()
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
    }
}