using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IEvaluateSubmit
    {
        Task<Evaluation> Evaluate(Submit submit);
    }
}
