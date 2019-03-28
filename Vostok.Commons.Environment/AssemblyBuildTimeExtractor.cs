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
            return ExtractFromAssembly(Assembly.GetEntryAssembly());
        }

        [CanBeNull]
        public static DateTime? ExtractFromAssembly(Assembly assembly)
        {
            var assemblyTitle = AssemblyTitleParser.GetAssemblyTitle(assembly);

            return ExtractFromTitle(assemblyTitle);
        }

        [CanBeNull]
        public static DateTime? ExtractFromAssembly(string assemblyPath)
        {
            var version = AssemblyTitleParser.GetAssemblyFileVersion(assemblyPath);

            return ExtractFromTitle(version?.FileDescription);
        }

        private static DateTime? ExtractFromTitle(string title)
        {
            return title == null ? null : AssemblyTitleParser.ParseBuildDate(title);
        }
    }
}