namespace Bhasha.Common.Services
{
    public interface ICheckResult
    {
        Result Evaluate(string expected, string actual);
    }

    public class ResultChecker : ICheckResult
    {
        public Result Evaluate(string expected, string actual)
        {
            // TODO
            // consider partially correct solution
            // https://en.wikipedia.org/wiki/Levenshtein_distance

            return actual == expected ? Result.Correct : Result.Wrong;
        }
    }
}
