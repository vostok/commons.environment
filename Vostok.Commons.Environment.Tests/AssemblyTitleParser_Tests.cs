using System;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Commons.Environment.Tests
{
    [TestFixture]
    internal class AssemblyTitleParser_Tests
    {
        public const string ValidTitle = @"
Commit: c2c780c5a3c3b58f072a93b89e508d0a622aa332 
Author: something
Date: 2021-06-08 16:58:19 +0500 
Ref names:  (HEAD -> master, origin/master, origin/HEAD)
Build date: 2021-07-02T15:24:38.3740000+05:00";
        
        [Test]
        public static void AssemblyTitleParser_should_parse_valid_title()
        {
            AssemblyTitleParser.ParseCommitHash(ValidTitle).Should().Be("c2c780c5a3c3b58f072a93b89e508d0a622aa332");
            AssemblyTitleParser.ParseBuildDate(ValidTitle).Should().Be(new DateTime(2021, 07, 02, 15, 24, 38, 374, DateTimeKind.Local));
            AssemblyTitleParser.ParseCommitTime(ValidTitle).Should().Be(new DateTime(2021, 06, 08, 16, 58, 19, DateTimeKind.Local));
        }
    }
}