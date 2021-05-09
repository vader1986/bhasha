using System;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Represents a <see cref="Chapter"/> by only holding the minimum amount of
    /// information, references (IDs) rather than actual objects.
    /// </summary>
    public class DbChapter : ICanBeValidated, IEntity
    {
        /// <summary>
        /// Reference to the <see cref="Chapter"/>.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Minimum user profile level required to go through this chapter.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Reference to the <see cref="Expression"/> for the name of the chapter.
        /// </summary>
        public Guid NameId { get; set; }

        /// <summary>
        /// Reference to the <see cref="Expression"/> for the description of the
        /// chapter.
        /// </summary>
        public Guid DescriptionId { get; set; }

        /// <summary>
        /// List of <see cref="PageDto">page references</see> for this chapter.
        /// </summary>
        public DbPage[]? Pages { get; set; }

        /// <summary>
        /// Link to an image representing the content of the chapter (optional).
        /// </summary>
        public ResourceId? PictureId { get; set; }

        public void Validate()
        {
            if (Level < 0 || Pages == null || Pages.Length == 0)
            {
                throw new InvalidObjectException(this);
            }
        }
    }
}
