using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Trillium.Syntax;
using Xunit;

namespace Trillium.Test.Syntax
{
    /// <summary>
    /// Source nested deeper than SyntaxTree.MaxNestingDepth is refused with a diagnostic instead of overflowing the
    /// stack (which ends the process and cannot be caught). Each parse runs on a thread with a 1 MB stack, the size of an
    /// ordinary thread, so a regression shows here as a crashed test run.
    /// </summary>
    public class NestingDepthTests
    {
        private static string Repeat(string s, int n) => string.Concat(Enumerable.Repeat(s, n));

        private static SyntaxTree ParseOnOneMegabyteStack(string text)
        {
            SyntaxTree tree = null;
            var thread = new Thread(() => tree = SyntaxTree.Parse(text, preventLoopsAndRecursion: true), 1024 * 1024);
            thread.Start();
            thread.Join();
            return tree;
        }

        public static TheoryData<string, string> DeepSources => new TheoryData<string, string>
        {
            { "parentheses", Repeat("(", 100_000) },
            { "calls", Repeat("a(", 100_000) },
            { "unary operators", Repeat("-", 100_000) + "1" },
            { "assignments", Repeat("a = ", 100_000) + "1" },
            { "blocks", Repeat("{", 100_000) },
            { "if statements", Repeat("if true ", 100_000) + "a = 1" },
            { "else-if chain", "if true { }" + Repeat(" else if true { }", 100_000) },
            { "binary right operands", Repeat("a + -(", 100_000) },
        };

        [Theory]
        [MemberData(nameof(DeepSources))]
        public void DeeplyNestedSource_IsRefused_NotAStackOverflow(string shape, string text)
        {
            var watch = Stopwatch.StartNew();
            var tree = ParseOnOneMegabyteStack(text);
            watch.Stop();

            Assert.NotNull(tree);
            Assert.Contains(tree.Diagnostics, d => d.Message == SyntaxTree.NestingTooDeepMessage);
            Assert.Single(tree.Diagnostics, d => d.Message == SyntaxTree.NestingTooDeepMessage);
            Assert.True(watch.Elapsed < TimeSpan.FromSeconds(5), $"{shape}: parse took {watch.Elapsed}");
        }

        [Fact]
        public void OrdinaryNesting_ParsesAsBefore()
        {
            var parens = Repeat("(", 100) + "1" + Repeat(")", 100);
            var elseIfs = "if a == 0 { b = 0 }" + string.Concat(Enumerable.Range(1, 50).Select(i => $" else if a == {i} {{ b = {i} }}"));
            foreach (var text in new[] { $"var a = {parens}", $"var a = 1\nvar b = 0\n{elseIfs}" })
            {
                var tree = ParseOnOneMegabyteStack(text);
                Assert.Empty(tree.Diagnostics);
            }
        }
    }
}
