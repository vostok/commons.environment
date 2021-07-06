using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    [PublicAPI]
    internal static class AssemblyCommitTimeExtractor
    {
        [CanBeNull]
        public static DateTimeOffset? ExtractFromEntryAssembly()
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
        public static DateTimeOffset? ExtractFromAssembly(Assembly assembly)
        {
            try
            {
                if (assembly == null)
                    return null;
                var assemblyTitle = AssemblyTitleParser.GetAssemblyTitle(assembly);
                return ExtractFromTitle(assemblyTitle);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [CanBeNull]
        public static DateTimeOffset? ExtractFromAssembly(string assemblyPath)
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

        private static DateTimeOffset? ExtractFromTitle(string title)
        {
            return title == null ? null : AssemblyTitleParser.ParseCommitTime(title);
        }
    }
}