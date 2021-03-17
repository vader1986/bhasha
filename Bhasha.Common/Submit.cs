using System;

namespace Bhasha.Common
{
    public class Submit
    {
        /// <summary>
        /// Reference to the user who submitted the solution.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Reference to the user profile to submit the solution for. The profile
        /// is required during the validation to determine the language.
        /// </summary>
        public Guid ProfileId { get; }

        /// <summary>
        /// Reference to the chapter the solution is submitted for. 
        /// </summary>
        public Guid ChapterId { get; }

        /// <summary>
        /// Index of the page the solution is submitted for. 
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// The actual solution the user submits.
        /// </summary>
        public string Solution { get; }

        public Submit(Guid userId, Guid profileId, Guid chapterId, int pageIndex, string solution)
        {
            UserId = userId;
            ProfileId = profileId;
            ChapterId = chapterId;
            PageIndex = pageIndex;
            Solution = solution;
        }
    }
}
