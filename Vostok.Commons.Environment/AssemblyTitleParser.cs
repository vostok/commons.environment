using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    internal static class AssemblyTitleParser
    {
        private static readonly Regex BuildDateRegex = new Regex(@"^(Build date:) (.*)$", RegexOptions.Multiline);
        private static readonly Regex CommitHashRegex = new Regex(@"^(Commit:|git) (.*)$", RegexOptions.Multiline);

        [CanBeNull]
        public static FileVersionInfo GetAssemblyFileVersion(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
                return null;

            try
            {
                return FileVersionInfo.GetVersionInfo(assemblyPath);
            }
            catch
            {
                return null;
            }
        }

        [CanBeNull]
        public static string GetAssemblyTitle(Assembly assembly)
        {
            var titleAttribute = assembly
                .GetCustomAttributes(true)
                .OfType<AssemblyTitleAttribute>()
                .SingleOrDefault();
            return titleAttribute?.Title;
        }

        [CanBeNull]
        public static string GetAssemblyInformationalVersion(Assembly assembly)
        {
            var titleAttribute = assembly
                .GetCustomAttributes(true)
                .OfType<AssemblyInformationalVersionAttribute>()
                .SingleOrDefault();
            return titleAttribute?.InformationalVersion;
        }

        [CanBeNull]
        public static DateTime? ParseBuildDate(string title)
        {
            try
            {
                var buildTimeString = GetCapturedGroupOrNull(BuildDateRegex, title);
                return DateTime.ParseExact(buildTimeString, "O", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [CanBeNull]
        public static string ParseCommitHash(string title)
        {
            try
            {
                var commitHash = GetCapturedGroupOrNull(CommitHashRegex, title);

                return commitHash?.ToLower();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GetCapturedGroupOrNull(Regex regex, string title)
        {
            var match = regex.Match(title);
            if (!match.Success)
            {
                return null;
            }

            return match.Groups[2].Value.Trim();
        }
    }
}