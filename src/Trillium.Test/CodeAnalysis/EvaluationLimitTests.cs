using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Trillium.CodeAnalysis;
using Trillium.Symbols;
using Trillium.Syntax;
using Xunit;

namespace Trillium.Test.CodeAnalysis
{
    /// <summary>
    /// A run that nests past EvaluationLimitException.MaxDepth or calls more than MaxCalls functions stops with that
    /// exception instead of overflowing the stack (which ends the process) or running for days. A Release build runs on a
    /// 1 MB stack, the size of an ordinary thread, so a regression shows as a crashed test run; a Debug build's frames are
    /// about five times larger, so it gets 4 MB.
    /// </summary>
    public class EvaluationLimitTests
    {
#if DEBUG
        private const int StackSize = 4 * 1024 * 1024;
#else
        private const int StackSize = 1024 * 1024;
#endif

        private static string Name(int i) { var s = ""; do { s = (char)('a' + i % 26) + s; i /= 26; } while (i > 0); return s; }

        /// <summary>Fa calls Fb calls ... n deep, each call under <paramref name="unary"/> unary minus signs.</summary>
        private static string Chain(int n, int unary = 0) =>
            string.Concat(Enumerable.Range(0, n).Select(i => $"function F{Name(i)}() : int {{ return {string.Concat(Enumerable.Repeat("- ", unary))}F{Name(i + 1)}() }}\n"))
            + $"function F{Name(n)}() : int {{ return 1 }}\n";

        /// <summary>Each level calls the next twice: 2^d calls.</summary>
        private static string Fan(int d) =>
            string.Concat(Enumerable.Range(0, d).Select(i => $"function F{Name(i)}() : int {{ return F{Name(i + 1)}() + F{Name(i + 1)}() }}\n"))
            + $"function F{Name(d)}() : int {{ return 1 }}\n";

        /// <summary>Declares the source, then evaluates Fa() on its own thread: its value, or the exception that stopped it.</summary>
        private static (object Value, Exception Error) RunFa(string source)
        {
            object value = null;
            Exception error = null;
            var thread = new Thread(() =>
            {
                try
                {
                    var tree = SyntaxTree.Parse(source, preventLoopsAndRecursion: true);
                    Assert.Empty(tree.Diagnostics);
                    var variables = new Dictionary<VariableSymbol, object>();
                    var declared = Compilation.CreateScript(null, tree);
                    declared.Evaluate(variables);
                    value = Compilation.CreateScript(declared, SyntaxTree.Parse("Fa()", preventLoopsAndRecursion: true)).Evaluate(variables).Value;
                }
                catch (Exception e) { error = e; }
            }, StackSize);
            thread.Start();
            thread.Join();
            return (value, error);
        }

        public static TheoryData<string, string> TooCostly => new TheoryData<string, string>
        {
            { "chain", Chain(3_000) },
            { "chain under unary operators", Chain(150, unary: 200) },
            { "fan", Fan(40) },
        };

        [Theory]
        [MemberData(nameof(TooCostly))]
        public void ACostlyRun_StopsWithTheLimit_NotAStackOverflowOrAHang(string shape, string source)
        {
            var watch = Stopwatch.StartNew();
            var (_, error) = RunFa(source);
            watch.Stop();

            Assert.IsType<EvaluationLimitException>(error);
            Assert.True(watch.Elapsed < TimeSpan.FromSeconds(10), $"{shape}: run took {watch.Elapsed}");
        }

        [Fact]
        public void OrdinaryRuns_AreUnchanged()
        {
            foreach (var (source, expected) in new[] { (Chain(100), 1), (Chain(500), 1), (Chain(2, unary: 200), 1), (Fan(15), 1 << 15) })
            {
                var (value, error) = RunFa(source);
                Assert.Null(error);
                Assert.Equal(expected, value);
            }
        }
    }
}
