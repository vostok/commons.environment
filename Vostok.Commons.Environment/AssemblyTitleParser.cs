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
            try
            {
                return !File.Exists(assemblyPath)
                    ? null
                    : FileVersionInfo.GetVersionInfo(assemblyPath);
            }
            catch
            {
                return null;
            }
        }

        [CanBeNull]
        public static string GetAssemblyTitle([NotNull] Assembly assembly)
        {
            try
            {
                var titleAttribute = assembly
                    .GetCustomAttributes(true)
                    .OfType<AssemblyTitleAttribute>()
                    .SingleOrDefault();
                return titleAttribute?.Title;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [CanBeNull]
        public static string GetAssemblyInformationalVersion([NotNull] Assembly assembly)
        {
            try
            {
                var titleAttribute = assembly
                    .GetCustomAttributes(true)
                    .OfType<AssemblyInformationalVersionAttribute>()
                    .SingleOrDefault();
                return titleAttribute?.InformationalVersion;
            }
            catch (Exception)
            {
                return null;
            }
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
            return !match.Success ? null : match.Groups[2].Value.Trim();
        }
    }
}