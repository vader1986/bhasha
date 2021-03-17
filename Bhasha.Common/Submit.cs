using System;

namespace Bhasha.Common
{
    public class Submit
    {
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

        public Submit(Guid chapterId, int pageIndex, string solution)
        {
            ChapterId = chapterId;
            PageIndex = pageIndex;
            Solution = solution;
        }
    }
}
