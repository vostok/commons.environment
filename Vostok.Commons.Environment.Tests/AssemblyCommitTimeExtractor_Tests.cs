using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    [TestFixture]
    internal class AssemblyCommitTimeExtractor_Tests
    {
        [Test]
        public static void ExtractFromAssembly_should_be_not_null()
            => AssemblyCommitTimeExtractor.ExtractFromAssembly(
                    Assembly.GetAssembly(typeof(AssemblyCommitHashExtractor)))
                .Should()
                .NotBeNull();

        [Test]
        public static void ExtractFromAssembly_by_path_should_be_not_null()
            => AssemblyCommitTimeExtractor.ExtractFromAssembly(
                    Assembly.GetAssembly(typeof(AssemblyCommitHashExtractor)).Location)
                .Should()
                .NotBeNull();
    }
}