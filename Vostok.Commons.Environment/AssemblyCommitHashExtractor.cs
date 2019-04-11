using System;
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
        public static string ExtractFromAssembly(Assembly assembly)
        {
            try
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
            catch (Exception)
            {
                return null;
            }
        }

        [CanBeNull]
        public static string ExtractFromAssembly(string assemblyPath)
        {
            try
            {
                var version = AssemblyTitleParser.GetAssemblyFileVersion(assemblyPath);

                var commitHash = ExtractFromTitle(version?.FileDescription);
                if (!string.IsNullOrEmpty(commitHash))
                    return commitHash;

                return ExtractFromTitle(version?.ProductVersion);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string ExtractFromTitle(string title)
        {
            return title == null ? null : AssemblyTitleParser.ParseCommitHash(title);
        }
    }
}