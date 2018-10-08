using System;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Vostok.Commons.Environment
{
    /// <summary>
    /// Determines the runtime on which the application is running.
    /// </summary>
    [PublicAPI]
    internal static class RuntimeDetector
    {
        /// <summary>
        /// Returns <c>true</c> when the application is running on Mono
        /// </summary>
        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;
     
        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core
        /// </summary>
        public static bool IsDotNetCore { get; } = RuntimeEnvironment.GetRuntimeDirectory().Contains("NETCore");
        
        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Framework
        /// </summary>
        public static bool IsDotNetFramework { get; } = RuntimeEnvironment.GetRuntimeDirectory().Contains(@"Microsoft.NET\Framework");
        
        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core 2.0
        /// </summary>
        public static bool IsDotNetCore20 { get; } = IsDotNetCore && RuntimeEnvironment.GetRuntimeDirectory().Contains($"{Path.DirectorySeparatorChar}2.0.");
        
        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core 2.1
        /// </summary>
        public static bool IsDotNetCore21 { get; } = IsDotNetCore && RuntimeEnvironment.GetRuntimeDirectory().Contains($"{Path.DirectorySeparatorChar}2.1.");

        // CR(iloktionov): This will likely stop working in .NET Core 3+
        /// <summary>
        /// Returns <c>true</c> when the application is running on .NET Core 2.1.0 or newer
        /// </summary>
        public static bool IsDotNetCore21AndNewer { get; } = IsDotNetCore && RuntimeEnvironment.GetRuntimeDirectory().Contains($"{Path.DirectorySeparatorChar}2.") && !IsDotNetCore20;

    }
}