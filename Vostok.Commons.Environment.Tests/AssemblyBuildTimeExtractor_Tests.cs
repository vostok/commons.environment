using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    [TestFixture]
    internal class AssemblyBuildTimeExtractor_Tests
    {
        [Test]
        public static void ExtractFromAssembly_should_be_not_null()
            => AssemblyBuildTimeExtractor.ExtractFromAssembly(
                    Assembly.GetAssembly(typeof(AssemblyCommitHashExtractor)))
                .Should()
                .NotBeNull();

        [Test]
        public static void ExtractFromAssembly_by_path_should_be_not_null()
            => AssemblyBuildTimeExtractor.ExtractFromAssembly(
                    Assembly.GetAssembly(typeof(AssemblyCommitHashExtractor)).Location)
                .Should()
                .NotBeNull();
    }
}