using System;
using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IUpdateStatsOnTipRequest
    {
        /// <summary>
        /// Updates the user <see cref="Stats"/> for the specified chapter by
        /// incrementing the number of tips used for the specified page.
        /// </summary>
        Task Update(Profile profile, Guid chapterId, int pageIndex);
    }
}
