using System;

namespace Bhasha.Common
{
    public class Submit : IEquatable<Submit?>
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
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            ChapterId = chapterId;
            PageIndex = pageIndex;
            Solution = solution;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Submit);
        }

        public bool Equals(Submit? other)
        {
            return other != null && ChapterId.Equals(other.ChapterId) && PageIndex == other.PageIndex && Solution == other.Solution;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ChapterId, PageIndex, Solution);
        }

        public static bool operator ==(Submit? left, Submit? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Submit? left, Submit? right)
        {
            return !(left == right);
        }
    }
}
