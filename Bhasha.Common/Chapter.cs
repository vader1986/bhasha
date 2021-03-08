using System;

namespace Bhasha.Common
{
    public class Chapter
    {
        /// <summary>
        /// Unique identifier for this chapter.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Minimum user profile level required to go through this chapter.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Name of the chapter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Short description about the content of the chapter.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Actual content of the chapter.
        /// </summary>
        public Page[] Pages { get; }

        /// <summary>
        /// Link to an image representing the content of the chapter (optional).
        /// </summary>
        public ResourceId? PictureId { get; }

        public Chapter(Guid id, int level, string name, string description, Page[] pages, ResourceId? pictureId)
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
