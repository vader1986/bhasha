using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    /// <summary>
    /// Defines an interface for the evaluation of user submits.
    /// </summary>
    public interface IEvaluateSubmit
    {
        /// <summary>
        /// Generates an <see cref="Evaluation"/> for the specified <see cref="Submit"/>
        /// of the specified user <see cref="Profile"/>.
        /// </summary>
        /// <param name="profile">User profile of the submit.</param>
        /// <param name="submit">Data submitted by the user.</param>
        /// <returns>The result of the evaluation.</returns>
        Task<Evaluation> Evaluate(Profile profile, Submit submit);
    }
}
