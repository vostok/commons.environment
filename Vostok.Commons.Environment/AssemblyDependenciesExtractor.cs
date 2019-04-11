using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    [PublicAPI]
    public static class AssemblyDependenciesExtractor
    {
        [NotNull]
        public static List<string> ExtractFromEntryAssembly()
        {
            try
            {
                return ExtractFromAssembly(Assembly.GetEntryAssembly());
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        [NotNull]
        public static List<string> ExtractFromAssembly(Assembly assembly)
        {
            try
            {
                return ExtractFromAssembly(assembly.Location);
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        [NotNull]
        public static List<string> ExtractFromAssembly(string assemblyPath)
        {
            try
            {
                return ExtractDependencies(assemblyPath).Select(dep => dep.FormatString()).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        private static List<Dependency> ExtractDependencies(string path)
        {
            var deps = new List<Dependency>();

            var directory = new DirectoryInfo(Path.GetDirectoryName(path) ?? string.Empty);
            foreach (var file in directory.EnumerateFiles().Where(file => (file.Extension == ".dll" || file.Extension == ".exe") && file.FullName != path))
            {
                var dep = TryGetDependency(file.FullName);
                if (dep != null)
                    deps.Add(dep);
            }

            return deps;
        }

        private static Dependency TryGetDependency(string assemblyPath)
        {
            var info = FileVersionInfo.GetVersionInfo(assemblyPath);

            var buildDate = AssemblyBuildTimeExtractor.ExtractFromAssembly(assemblyPath);
            var commitHash = AssemblyCommitHashExtractor.ExtractFromAssembly(assemblyPath);

            if (commitHash != null)
            {
                return new Dependency
                {
                    CommitHash = commitHash,
                    Name = Path.GetFileNameWithoutExtension(info.OriginalFilename),
                    Date = buildDate
                };
            }

            return null;
        }

        private class Dependency
        {
            public string Name;
            public string CommitHash;
            public DateTime? Date;

            public string FormatString()
            {
                return $"{Name?.Replace(' ', '_').Replace(';', '_')} {CommitHash} {Date?.ToString("dd.MM.yyyy")}";
            }
        }
    }
}