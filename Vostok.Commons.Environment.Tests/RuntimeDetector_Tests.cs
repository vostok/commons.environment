using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    public class RuntimeDetector_Tests
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
        public void Should_detect_Core21_and_newer() => RuntimeDetector.IsDotNetCore21AndNewer.Should().BeTrue();
        #endif
    }
}