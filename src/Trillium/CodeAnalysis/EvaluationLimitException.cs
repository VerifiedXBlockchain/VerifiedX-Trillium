namespace Trillium.CodeAnalysis
{
    /// <summary>
    /// Thrown when a run nests expressions and calls deeper than <see cref="MaxDepth"/> or makes more than
    /// <see cref="MaxCalls"/> function calls. Code has no loops or recursion, but a long chain of calls would otherwise
    /// overflow the stack (which ends the process and cannot be caught), and a function calling the next one twice per
    /// level would run for days. Both are counted, not timed, so every machine stops the same code at the same point.
    /// A Release build holds over 1,500 chained calls (the deepest kind of level) on a 1 MB stack.
    /// </summary>
    public sealed class EvaluationLimitException : Exception
    {
        public const int MaxDepth = 512;
        public const long MaxCalls = 1_000_000;

        private EvaluationLimitException(string message) : base(message) { }

        // Built here, not where they are thrown: the evaluator's recursive frames stay small.
        internal static EvaluationLimitException TooDeep() =>
            new EvaluationLimitException($"Code nests expressions and calls more than {MaxDepth} deep.");

        internal static EvaluationLimitException TooManyCalls() =>
            new EvaluationLimitException($"Code makes more than {MaxCalls} function calls.");
    }
}
