using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation of the user <see cref="Profile"/>.
    /// </summary>
    public class DbUserProfile : ICanBeValidated, IEntity, IEquatable<DbUserProfile?>
    {
        /// <summary>
        /// Unqiue identifier of the profile.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique identifier of the user owning the profile.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Language profile including native- and target-language.
        /// </summary>
        public DbProfile? Languages { get; set; }

        /// <summary>
        /// Accomplished level for the user profile.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Number of completed chapters for this user profile.
        /// </summary>
        public int CompletedChapters { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(UserId) || Languages == null || Level < 0 || CompletedChapters < 0)
            {
                throw new InvalidObjectException(this);
            }

            Languages.Validate();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbUserProfile);
        }

        public bool Equals(DbUserProfile? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   UserId == other.UserId &&
                   Languages == other.Languages &&
                   Level == other.Level &&
                   CompletedChapters == other.CompletedChapters;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserId, Languages, Level, CompletedChapters);
        }

        public static bool operator ==(DbUserProfile? left, DbUserProfile? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbUserProfile? left, DbUserProfile? right)
        {
            return !(left == right);
        }
    }
}
