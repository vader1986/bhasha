using System;

namespace Bhasha.Common
{
    public class Tip
    {
        /// <summary>
        /// Unique identifier of the tip.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Reference to the chapter.
        /// </summary>
        public Guid ChapterId { get; }

        /// <summary>
        /// Index of the page within the chapter this tip is created for.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Content of the tip.
        /// </summary>
        public string Text { get; }

        public Tip(Guid id, Guid chapterId, int pageIndex, string text)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            Id = id;
            ChapterId = chapterId;
            PageIndex = pageIndex;
            Text = text;
        }
    }
}
