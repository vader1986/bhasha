using System;

namespace Bhasha.Common
{
    public class Evaluation : IEquatable<Evaluation>
    {
        /// <summary>
        /// Evaluation <see cref="Common.Result"/> for a <see cref="Submit"/>.
        /// </summary>
        public Result Result { get; }

        /// <summary>
        /// Original submit of the evaluation.
        /// </summary>
        public Submit Submit { get; }

        public Evaluation(Result result, Submit submit)
        {
            Result = result;
            Submit = submit;
        }

        public override string ToString()
        {
            return $"{nameof(Result)}: {Result}, {nameof(Submit)}: {Submit}";
        }

        public bool Equals(Evaluation other)
        {
            return other != null && other.Result == Result && other.Submit.Equals(Submit);
        }
    }
}
