namespace Trillium.CodeAnalysis
{
    /// <summary>
    /// What one run may do, counted and never timed, so every machine stops the same code at the same point. Over a limit
    /// a run stops with <see cref="EvaluationLimitException"/>.
    /// <list type="bullet">
    /// <item><see cref="MaxSteps"/>: statements and expressions evaluated. It bounds the time of code that loops without
    /// calling anything.</item>
    /// <item><see cref="MaxStringLength"/>: the length of any one string the run makes. It is checked before the string is
    /// made, so a run that doubles a string line after line stops before it holds the large one.</item>
    /// <item><see cref="MaxTextWork"/>: characters of text the run makes, compares, converts, splits or signs, all
    /// counted together. It bounds the memory and the time a run spends on text, whatever the shape of the code.</item>
    /// </list>
    /// The nesting depth and call count of <see cref="EvaluationLimitException"/> hold for every run.
    /// </summary>
    public sealed class EvaluationLimits
    {
        public long MaxSteps { get; }
        public int MaxStringLength { get; }
        public long MaxTextWork { get; }

        public EvaluationLimits(long maxSteps, int maxStringLength, long maxTextWork)
        {
            MaxSteps = maxSteps;
            MaxStringLength = maxStringLength;
            MaxTextWork = maxTextWork;
        }

        /// <summary>The limits a run has unless its host asks for others.</summary>
        public static EvaluationLimits Default { get; } = new EvaluationLimits(10_000_000, 4 * 1024 * 1024, 32L * 1024 * 1024);

        /// <summary>
        /// No step or text limit: a run as before 1.9.0. For a host that must reproduce what older versions did with code
        /// it accepted then; nothing else should use it.
        /// </summary>
        public static EvaluationLimits None { get; } = new EvaluationLimits(long.MaxValue, int.MaxValue, long.MaxValue);
    }
}
