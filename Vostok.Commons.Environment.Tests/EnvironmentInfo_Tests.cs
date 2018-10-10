using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    public class EnvironmentInfo_Tests
    {
        [Test]
        public static void Application_name_should_be_not_null_or_empty()
            => string.IsNullOrEmpty(EnvironmentInfo.Application).Should().BeFalse();
    
        [Test]
        public static void Host_name_should_be_not_null_or_empty()
            => string.IsNullOrEmpty(EnvironmentInfo.Application).Should().BeFalse();
    }
}