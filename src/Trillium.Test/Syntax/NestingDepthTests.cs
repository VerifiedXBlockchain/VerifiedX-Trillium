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
            // a + b + c is (a + b) + c: each operator in a chain is one level deeper, though the parser loops over them.
            { "operator chain", "1" + Repeat("+1", 32_000) },
            { "operator chain with a syntax error after it", "1" + Repeat("+1", 32_000) + "@" },
            { "mixed operator chain", "1" + Repeat("*1+1", 16_000) },
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
            var chain = "1" + Repeat(" + 1", 200);
            foreach (var text in new[] { $"var a = {parens}", $"var a = 1\nvar b = 0\n{elseIfs}", $"var a = {chain}" })
            {
                var tree = ParseOnOneMegabyteStack(text);
                Assert.Empty(tree.Diagnostics);
            }
        }

        [Fact]
        public void Descendants_VisitEveryNodeInSourceOrder()
        {
            var tree = SyntaxTree.Parse("function F(a : int) : int { var b = a * 2 + 1 if b > 3 { return b } return -a }\nvar c = F(1) + F(2)");
            Assert.Empty(tree.Diagnostics);
            var expected = new System.Collections.Generic.List<SyntaxNode>();
            void Walk(SyntaxNode node) { expected.Add(node); foreach (var child in node.GetChildren()) Walk(child); }
            Walk(tree.Root);
            Assert.Equal(expected, TrilliumTest.Extensions.SyntaxNodeExtensions.Descendants(tree.Root).ToList());
        }
    }
}
