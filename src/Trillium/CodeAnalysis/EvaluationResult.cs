using System.Collections.Immutable;

namespace Trillium.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object value)
        {
            Diagnostics = diagnostics;
            Value = value;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public object Value { get; }

        /// <summary>What the run used of its <see cref="EvaluationLimits"/>: steps taken.</summary>
        public long Steps { get; init; }

        /// <summary>What the run used of its <see cref="EvaluationLimits"/>: characters of text worked through.</summary>
        public long TextWork { get; init; }
    }
}
