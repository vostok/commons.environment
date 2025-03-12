using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    [PublicAPI]
    internal static class AssemblyCommitHashExtractor
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

                var commitHash = GetCommitHashFromAssemblyInformationalVersion(assembly);
                if (commitHash != null)
                    return commitHash;

                var assemblyTitle = AssemblyTitleParser.GetAssemblyTitle(assembly);
                commitHash = ExtractFromTitle(assemblyTitle);
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
        
        private static string GetCommitHashFromAssemblyInformationalVersion(Assembly assembly)
        {
            try
            {
                var informationalVersion = assembly.GetCustomAttributes(true)
                    .OfType<AssemblyInformationalVersionAttribute>()
                    .SingleOrDefault()
                    ?.InformationalVersion;

                if (informationalVersion != null)
                {
                    var versionAndCommit = informationalVersion.Split(["+"], StringSplitOptions.RemoveEmptyEntries);
                    if (versionAndCommit.Length == 2)
                        return versionAndCommit[1];
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}