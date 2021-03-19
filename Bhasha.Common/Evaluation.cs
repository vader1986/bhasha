namespace Bhasha.Common
{
    public class Evaluation
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
    }
}
