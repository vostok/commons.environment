using System;
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
            => string.IsNullOrEmpty(EnvironmentInfo.Host).Should().BeFalse();

        [Test]
        public void FQDN_should_not_be_null_or_empty()
        {
            EnvironmentInfo.FQDN.Should().NotBeNullOrEmpty();

            Console.Out.WriteLine(EnvironmentInfo.FQDN);
        }

        [Test]
        public void ServiceDiscoveryIPv4_should_be_null_if_environment_variable_is_not_set()
        {
            EnvironmentInfo.ServiceDiscoveryIPv4.Should().BeNull();

            Console.Out.WriteLine(EnvironmentInfo.ServiceDiscoveryIPv4);
        }

        [Test]
        public void HomeDirectory_should_not_be_null_or_empty()
        {
            EnvironmentInfo.HomeDirectory.Should().NotBeNullOrEmpty();

            Console.Out.WriteLine(EnvironmentInfo.HomeDirectory);
        }

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