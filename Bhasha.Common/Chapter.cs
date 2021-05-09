using System;
using System.Linq;

namespace Bhasha.Common
{
    public class Chapter : IEquatable<Chapter?>
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
        public TranslatedExpression Name { get; }

        /// <summary>
        /// Short description about the content of the chapter.
        /// </summary>
        public TranslatedExpression Description { get; }

        /// <summary>
        /// Actual content of the chapter.
        /// </summary>
        public Page[] Pages { get; }

        /// <summary>
        /// Link to an image representing the content of the chapter (optional).
        /// </summary>
        public ResourceId? PictureId { get; }

        public Chapter(Guid id, int level, TranslatedExpression name, TranslatedExpression description, Page[] pages, ResourceId? pictureId)
        {
            Id = id;
            Level = level;
            Name = name;
            Description = description;
            Pages = pages;
            PictureId = pictureId;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Level)}: {Level}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(Pages)}: [{string.Join(',', Pages?.Select(x => x.ToString()))}], {nameof(PictureId)}: {PictureId}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Chapter);
        }

        public bool Equals(Chapter? other)
        {
            return other != null && Id.Equals(other.Id) && Level == other.Level && Name == other.Name && Description == other.Description && Pages == other.Pages && PictureId == other.PictureId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Level, Name, Description, Pages, PictureId);
        }

        public static bool operator ==(Chapter? left, Chapter? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Chapter? left, Chapter? right)
        {
            return !(left == right);
        }
    }
}
