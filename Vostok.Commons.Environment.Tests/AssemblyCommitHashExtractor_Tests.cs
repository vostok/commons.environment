using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    [TestFixture]
    internal class AssemblyCommitHashExtractor_Tests
    {
        [Test]
        public static void ExtractFromAssembly_should_be_not_null_or_empty()
            => string.IsNullOrEmpty(
                    AssemblyCommitHashExtractor.ExtractFromAssembly(
                        Assembly.GetAssembly(typeof(AssemblyCommitHashExtractor))))
                .Should()
                .BeFalse();

        [Test]
        public static void ExtractFromAssembly_by_path_should_be_not_null_or_empty()
            => string.IsNullOrEmpty(
                    AssemblyCommitHashExtractor.ExtractFromAssembly(
                        Assembly.GetAssembly(typeof(AssemblyCommitHashExtractor)).Location))
                .Should()
                .BeFalse();
    }
}