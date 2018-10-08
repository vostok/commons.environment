using System;
using System.Diagnostics;
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
    internal static class ApplicationIdentity
    {
        private static Lazy<string> identity = new Lazy<string>(GetIdentity);

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