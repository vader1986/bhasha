using System;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation of the user <see cref="Profile"/>.
    /// </summary>
    public class DbUserProfile : ICanBeValidated, IEntity
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
    }
}
