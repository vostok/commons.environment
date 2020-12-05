using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    internal class RuntimeDetector_Tests
    {
#if NETFRAMEWORK
        [Test]
        public void Should_detect_Framework() => RuntimeDetector.IsDotNetFramework.Should().BeTrue();
    
        [Test]
        public void Should_return_false_for_Core_on_Framework() => RuntimeDetector.IsDotNetCore.Should().BeFalse();
    
        [Test]
        public void Should_return_false_for_Core20_on_Framework() => RuntimeDetector.IsDotNetCore20.Should().BeFalse();
    
        [Test]
        public void Should_return_false_for_Core21AndNewer_on_Framework() => RuntimeDetector.IsDotNetCore21AndNewer.Should().BeFalse();
#endif

#if NETCOREAPP
        [Test]
        public void Should_return_false_for_Framework_on_core() => RuntimeDetector.IsDotNetFramework.Should().BeFalse();

        [Test]
        public void Should_detect_Core() => RuntimeDetector.IsDotNetCore.Should().BeTrue();
#endif

#if NETCOREAPP2_1
        [Test]
        public void Should_return_false_for_Core20_on_Core21() => RuntimeDetector.IsDotNetCore20.Should().BeFalse();

        [Test]
        public void Should_return_false_for_Core30_on_Core21() => RuntimeDetector.IsDotNetCore30AndNewer.Should().BeFalse();

        [Test]
        public void Should_return_false_for_Core50_on_Core21() => RuntimeDetector.IsDotNet50AndNewer.Should().BeFalse();

        [Test]
        public void Should_detect_Core21_and_newer() => RuntimeDetector.IsDotNetCore21AndNewer.Should().BeTrue();
#endif

#if NETCOREAPP3_1
        [Test]
        public void Should_return_false_for_Core50_on_Core31() => RuntimeDetector.IsDotNet50AndNewer.Should().BeFalse();

        [Test]
        public void Should_detect_Core21_and_newer() => RuntimeDetector.IsDotNetCore21AndNewer.Should().BeTrue();

        [Test]
        public void Should_detect_Core30_and_newer() => RuntimeDetector.IsDotNetCore30AndNewer.Should().BeTrue();
#endif

#if NET5_0
        [Test]
        public void Should_detect_Core21_and_newer() => RuntimeDetector.IsDotNetCore21AndNewer.Should().BeTrue();

        [Test]
        public void Should_detect_Core30_and_newer() => RuntimeDetector.IsDotNetCore30AndNewer.Should().BeTrue();

        [Test]
        public void Should_detect_Core50_and_newer() => RuntimeDetector.IsDotNet50AndNewer.Should().BeTrue();
#endif
    }
}