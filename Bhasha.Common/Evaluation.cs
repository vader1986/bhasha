namespace Bhasha.Common
{
    public class Evaluation
    {
        /// <summary>
        /// Evaluation <see cref="Common.Result"/> for a <see cref="Submit"/>.
        /// </summary>
        public Result Result { get; }

        public Evaluation(Result result)
        {
            Result = result;
        }
    }
}
