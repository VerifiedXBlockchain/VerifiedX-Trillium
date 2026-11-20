using System;
using System.Collections.Generic;
using System.Linq;
using Trillium.CodeAnalysis;
using Trillium.Symbols;
using Trillium.Syntax;
using Xunit;

namespace Trillium.Test.CodeAnalysis
{
    /// <summary>
    /// getMethods() lists a contract's functions in one order on every node: by name, ordinally. It listed them in the
    /// order of the function table, which follows its symbols' hashes, and .NET seeds those afresh in every process, so
    /// the same contract gave each node its own answer and code that branched on it ran differently node to node.
    /// </summary>
    public class ContractMethodsTests
    {
        private static readonly string[] Names = { "Zeta", "Alpha", "Mid", "GetNFTFeatures", "beta", "Omega", "Delta", "Kappa" };

        private static string Declare(IEnumerable<string> names) =>
            string.Concat(names.Select(n => $"function {n}() : string {{ return \"{n}\" }}\n"));

        private static string Methods(string source)
        {
            var tree = SyntaxTree.Parse(source, preventLoopsAndRecursion: true);
            Assert.Empty(tree.Diagnostics);
            var variables = new Dictionary<VariableSymbol, object>();
            var declared = Compilation.CreateScript(null, tree);
            declared.Evaluate(variables);
            return (string)Compilation.CreateScript(declared, SyntaxTree.Parse("getMethods()", preventLoopsAndRecursion: true)).Evaluate(variables).Value;
        }

        [Fact]
        public void GetMethods_ListsTheFunctionsByName()
        {
            var expected = string.Join(",", Names.OrderBy(n => n, StringComparer.Ordinal).Select(n => $"{n}(): string"));
            Assert.Equal(expected, Methods(Declare(Names)));
        }

        [Fact]
        public void GetMethods_DoesNotDependOnTheOrderTheFunctionsAreDeclaredIn()
        {
            var forward = Methods(Declare(Names));
            Assert.Equal(forward, Methods(Declare(Names.Reverse())));
            Assert.Equal(forward, Methods(Declare(Names.OrderBy(n => n.Length).ThenByDescending(n => n, StringComparer.Ordinal))));
        }
    }
}
