using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IUpdateStatsOnSubmit
    {
        /// <summary>
        /// Updates user <see cref="Stats"/> for the chapter and page which is
        /// affected by the <see cref="Submit"/> of the specified <see cref="Evaluation"/>.
        /// Depending on the <see cref="Result"/> of the <see cref="Evaluation"/>
        /// this may affect <see cref="Stats.Completed"/>, <see cref="Stats.Submits"/>
        /// and <see cref="Stats.Failures"/>. Also, profile stats such as
        /// <see cref="Profile.CompletedChapters"/> and <see cref="Profile.Level"/>
        /// may be updated.
        /// </summary>
        /// <param name="evaluation">Evaluation for the submit.</param>
        /// <param name="pages">Number of pages of the current chapter used to
        /// create new <see cref="Stats"/> in case no user stats for the chapter
        /// are available yet (for the first submit).</param>
        /// <returns>The input evaluation with the updated profile.</returns>
        Task<Evaluation> Update(Evaluation evaluation, int pages);
    }
}
