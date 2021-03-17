namespace Bhasha.Common.Services
{
    public class SolutionEvaluator : IEvaluateSolution
    {
        public Evaluation Evaluate(string expected, string actual)
        {
            // TODO
            // consider partially correct solution
            // https://en.wikipedia.org/wiki/Levenshtein_distance

            return new Evaluation(actual == expected ? Result.Correct : Result.Wrong);
        }
    }
}
