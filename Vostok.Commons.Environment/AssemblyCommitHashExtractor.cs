using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    [PublicAPI]
    public static class AssemblyCommitHashExtractor
    {
        [CanBeNull]
        public static string ExtractFromEntryAssembly()
        {
            return ExtractFromAssembly(Assembly.GetEntryAssembly());
        }

        [CanBeNull]
        public static string ExtractFromAssembly(Assembly assembly)
        {
            if (assembly == null)
                return null;

            var assemblyTitle = AssemblyTitleParser.GetAssemblyTitle(assembly);
            var commitHash = ExtractFromTitle(assemblyTitle);
            if (!string.IsNullOrEmpty(commitHash))
                return commitHash;

            var productVersion = AssemblyTitleParser.GetAssemblyInformationalVersion(assembly);
            return ExtractFromTitle(productVersion);
        }

        [CanBeNull]
        public static string ExtractFromAssembly(string assemblyPath)
        {
            var version = AssemblyTitleParser.GetAssemblyFileVersion(assemblyPath);

            var commitHash = ExtractFromTitle(version?.FileDescription);
            if (!string.IsNullOrEmpty(commitHash))
                return commitHash;

            return ExtractFromTitle(version?.ProductVersion);
        }

        private static string ExtractFromTitle(string title)
        {
            return title == null ? null : AssemblyTitleParser.ParseCommitHash(title);
        }
    }
}