using System;

namespace Bhasha.Common
{
    public class GenericChapter
    {
        /// <summary>
        /// Unique identifier of the chapter.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Level of difficulty of the chapter.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Name of the chapter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Description of the chapter.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Sequence of pages of the chapter.
        /// </summary>
        public GenericPage[] Pages { get; }

        /// <summary>
        /// Link to an image representing the content of the chapter (optional).
        /// </summary>
        public ResourceId? PictureId { get; }

        public GenericChapter(Guid id, int level, string name, string description, GenericPage[] pages, ResourceId? pictureId)
        {
            Id = id;
            Level = level;
            Name = name;
            Description = description;
            Pages = pages;
            PictureId = pictureId;
        }
    }
}
