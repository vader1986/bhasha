namespace Bhasha.Common.Services
{
    public interface IEvaluateSolution
    {
        Evaluation Evaluate(string expected, string actual);
    }
}
