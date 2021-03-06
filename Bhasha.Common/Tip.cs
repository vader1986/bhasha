namespace Bhasha.Common
{
    public class Tip
    {
        /// <summary>
        /// Unique identifier of the tip.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Reference to the chapter.
        /// </summary>
        public int ChapterId { get; }

        /// <summary>
        /// Index of the page within the chapter this tip is created for.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Content of the tip.
        /// </summary>
        public string Text { get; }

        public Tip(int id, int chapterId, int pageIndex, string text)
        {
            Id = id;
            ChapterId = chapterId;
            PageIndex = pageIndex;
            Text = text;
        }
    }
}
