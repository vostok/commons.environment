using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    [PublicAPI]
    public static class AssemblyBuildTimeExtractor
    {
        [CanBeNull]
        public static DateTime? ExtractFromEntryAssembly()
        {
            try
            {
                return ExtractFromAssembly(Assembly.GetEntryAssembly());
            }
            catch (Exception)
            {
                return null;
            }
        }

        [CanBeNull]
        public static DateTime? ExtractFromAssembly(Assembly assembly)
        {
            try
            {
                var assemblyTitle = AssemblyTitleParser.GetAssemblyTitle(assembly);
                return ExtractFromTitle(assemblyTitle);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [CanBeNull]
        public static DateTime? ExtractFromAssembly(string assemblyPath)
        {
            try
            {
                var version = AssemblyTitleParser.GetAssemblyFileVersion(assemblyPath);
                return ExtractFromTitle(version?.FileDescription);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static DateTime? ExtractFromTitle(string title)
        {
            return title == null ? null : AssemblyTitleParser.ParseBuildDate(title);
        }
    }
}