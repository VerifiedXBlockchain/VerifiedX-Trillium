using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Trillium.CodeAnalysis;
using Trillium.Symbols;
using Trillium.Syntax;
using Xunit;

namespace Trillium.Test.CodeAnalysis
{
    /// <summary>
    /// A run stops at <see cref="EvaluationLimits"/>: the steps it takes, the length of any one string it makes, and the
    /// text it works through. Code that builds a huge string with no loop and no call, loops without calling anything, or
    /// compares long strings over and over stops quickly and holds little, on every machine at the same point.
    /// </summary>
    public class EvaluationBudgetTests
    {
        /// <summary>Declares the source, then evaluates <paramref name="entry"/>: its value, or the exception that stopped it.</summary>
        private static (object Value, Exception Error) Run(string source, string entry, EvaluationLimits limits = null)
        {
            try
            {
                var tree = SyntaxTree.Parse(source, preventLoopsAndRecursion: true);
                Assert.Empty(tree.Diagnostics);
                var variables = new Dictionary<VariableSymbol, object>();
                var declared = Compilation.CreateScript(null, tree);
                var first = limits == null ? declared.Evaluate(variables) : declared.Evaluate(variables, limits);
                Assert.Empty(first.Diagnostics);
                var call = Compilation.CreateScript(declared, SyntaxTree.Parse(entry, preventLoopsAndRecursion: true));
                return ((limits == null ? call.Evaluate(variables) : call.Evaluate(variables, limits)).Value, null);
            }
            catch (EvaluationLimitException e) { return (null, e); }
        }

        /// <summary>A 125-character string, then <paramref name="doublings"/> lines that double it.</summary>
        private static string Doubling(int doublings) =>
            "var payload = \"" + new string('x', 125) + "\"\n" + string.Concat(Enumerable.Repeat("payload = payload + payload\n", doublings));

        [Fact]
        public void A_string_doubled_line_after_line_stops_before_the_large_string_is_made()
        {
            GC.Collect();
            var before = GC.GetTotalAllocatedBytes(precise: true);
            var watch = Stopwatch.StartNew();
            var (_, error) = Run(Doubling(23) + "function Fa() : int { return 1 }\n", "Fa()"); // 2 GB of text without the limit
            Assert.IsType<EvaluationLimitException>(error);
            Assert.Contains("string longer than", error.Message);
            Assert.True(watch.Elapsed < TimeSpan.FromSeconds(5), $"took {watch.Elapsed}");
            var allocated = GC.GetTotalAllocatedBytes(precise: true) - before;
            Assert.True(allocated < 64L * 1024 * 1024, $"allocated {allocated / (1024 * 1024)} MB");
        }

        [Fact]
        public void A_loop_that_calls_nothing_stops_at_the_step_limit()
        {
            // do-while is not covered by the parser's loop ban.
            var watch = Stopwatch.StartNew();
            var (_, error) = Run("function Fa() : int { var i = 0 do { i = i + 1 } while true return i }\n", "Fa()");
            Assert.IsType<EvaluationLimitException>(error);
            Assert.Contains("steps", error.Message);
            Assert.True(watch.Elapsed < TimeSpan.FromSeconds(30), $"took {watch.Elapsed}");
        }

        [Fact]
        public void Long_strings_compared_over_and_over_stop_at_the_text_limit()
        {
            // Two different 1M-character strings equal in content, compared in a fan of calls (2^12 comparisons): no
            // string is made, but each comparison reads a million characters.
            var source = Doubling(13).Replace("var payload", "var a") .Replace("payload", "a")
                + "var b = a + \"\"\n"
                + string.Concat(Enumerable.Range(0, 12).Select(i => $"function F{(char)('a' + i)}() : int {{ return F{(char)('a' + i + 1)}() + F{(char)('a' + i + 1)}() }}\n"))
                + "function Fm() : int { if a == b { return 1 } return 0 }\n";
            var watch = Stopwatch.StartNew();
            var (_, error) = Run(source, "Fa()");
            Assert.IsType<EvaluationLimitException>(error);
            Assert.Contains("characters of text", error.Message);
            Assert.True(watch.Elapsed < TimeSpan.FromSeconds(10), $"took {watch.Elapsed}");
        }

        [Fact]
        public void A_run_reports_what_it_used()
        {
            var variables = new Dictionary<VariableSymbol, object>();
            var declared = Compilation.CreateScript(null, SyntaxTree.Parse("var a = \"abc\" + \"de\"", preventLoopsAndRecursion: true));
            var made = declared.Evaluate(variables, EvaluationLimits.Default);
            Assert.Empty(made.Diagnostics);
            Assert.True(made.Steps > 0);
            Assert.Equal(5, made.TextWork); // five characters made

            var compared = Compilation.CreateScript(declared, SyntaxTree.Parse("a == \"abcde\"", preventLoopsAndRecursion: true)).Evaluate(variables, EvaluationLimits.Default);
            Assert.Equal(true, compared.Value);
            Assert.Equal(5, compared.TextWork); // five compared: each run is counted on its own
        }

        [Fact]
        public void Ordinary_runs_are_unchanged_and_the_limits_can_be_lifted()
        {
            // A contract's getters: a few concatenations, conversions and comparisons.
            var source = "let Name = \"Art\"\nlet Supply = 21\nvar State = \"{*0}\"\n"
                + "function Main() : string { if Name == \"Art\" { State = \"{*\" + string(Supply) + \"}\" } return Name + \"|->\" + State }\n";
            Assert.Equal("Art|->{*21}", Run(source, "Main()").Value);
            Assert.Equal("Art|->{*21}", Run(source, "Main()", EvaluationLimits.Default).Value);

            // Ten doublings make 128,000 characters: inside the defaults, and the same without limits.
            var (made, error) = Run(Doubling(10) + "function Fa() : int { return 1 }\n", "payload");
            Assert.Null(error);
            Assert.Equal(125 * 1024, ((string)made).Length);
            var tight = new EvaluationLimits(maxSteps: 1_000, maxStringLength: 1_000, maxTextWork: 1_000_000);
            Assert.IsType<EvaluationLimitException>(Run(Doubling(10) + "function Fa() : int { return 1 }\n", "payload", tight).Error);
            Assert.Null(Run(Doubling(10) + "function Fa() : int { return 1 }\n", "payload", EvaluationLimits.None).Error);
        }
    }
}
