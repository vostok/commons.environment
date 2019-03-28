using System.Diagnostics;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    [TestFixture]
    internal class EnvironmentInfo_Tests
    {
        [Test]
        public static void Application_name_should_be_not_null_or_empty()
            => string.IsNullOrEmpty(EnvironmentInfo.Application).Should().BeFalse();

        [Test]
        public static void Host_name_should_be_not_null_or_empty()
            => string.IsNullOrEmpty(EnvironmentInfo.Application).Should().BeFalse();

        [Test]
        public static void ProcessName_should_be_current_process_name()
            => EnvironmentInfo.ProcessName.Should().Be(Process.GetCurrentProcess().ProcessName);

        [Test]
        public static void ProcessId_should_be_current_process_id()
            => EnvironmentInfo.ProcessId.Should().Be(Process.GetCurrentProcess().Id);

        [Test]
        public static void BaseDirectory_should_be_same_as_test_directory()
            => EnvironmentInfo.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar).Should().Be(TestContext.CurrentContext.TestDirectory);
    }
}