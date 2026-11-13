using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trillium.Syntax;

namespace TrilliumTest.Extensions
{
    public static class SyntaxNodeExtensions
    {
        /// <summary>
        /// The node and every node under it, each before its children, children in source order. Walked with a stack: a
        /// nested iterator per level made the walk cost depth times size and grow the call stack with the depth.
        /// </summary>
        public static IEnumerable<SyntaxNode> Descendants(this SyntaxNode node)
        {
            var pending = new Stack<SyntaxNode>();
            pending.Push(node);
            while (pending.Count > 0)
            {
                var current = pending.Pop();
                yield return current;
                var children = current.GetChildren().ToArray();
                for (var i = children.Length - 1; i >= 0; i--)
                    pending.Push(children[i]);
            }
        }
    }
}
