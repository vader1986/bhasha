using System;
using System.Linq;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedChapter : ICanBeValidated, IEntity, IEquatable<DbTranslatedChapter?>
    {
        /// <summary>
        /// Unqiue identifier of the chapter translation used as primary key
        /// for database storage. This ID has to match <see cref="DbChapter.Id"/>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Language profile including native- and target-language.
        /// </summary>
        public DbProfile? Languages { get; set; }

        /// <summary>
        /// Minimum user profile level required to go through this chapter.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Name of the chapter.
        /// </summary>
        public DbTranslatedExpression? Name { get; set; }

        /// <summary>
        /// Short description about the content of the chapter.
        /// </summary>
        public DbTranslatedExpression? Description { get; set; }

        /// <summary>
        /// Actual content of the chapter.
        /// </summary>
        public DbTranslatedPage[]? Pages { get; set; }

        /// <summary>
        /// Link to an image representing the content of the chapter (optional).
        /// </summary>
        public ResourceId? PictureId { get; set; }

        public void Validate()
        {
            if (Languages == null || Level < 0 || Name == null || Description == null || Pages == null || Pages.Length == 0)
            {
                throw new InvalidObjectException(this);
            }

            Languages.Validate();
            Name.Validate();
            Description.Validate();

            foreach (var page in Pages)
            {
                page.Validate();
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbTranslatedChapter);
        }

        public bool Equals(DbTranslatedChapter? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   Languages == other.Languages &&
                   Level == other.Level &&
                   Name == other.Name &&
                   Description == other.Description &&
                   Pages.SequenceEqual(other.Pages) &&
                   PictureId == other.PictureId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Languages, Level, Name, Description, Pages, PictureId);
        }

        public static bool operator ==(DbTranslatedChapter? left, DbTranslatedChapter? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbTranslatedChapter? left, DbTranslatedChapter? right)
        {
            return !(left == right);
        }
    }
}
