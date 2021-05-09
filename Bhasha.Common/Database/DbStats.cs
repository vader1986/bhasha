using System;
using System.Linq;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation for <see cref="Stats"/> associated with a user
    /// <see cref="Profile"/> and a <see cref="Chapter"/>.
    /// </summary>
    public class DbStats : ICanBeValidated, IEntity
    {
        /// <summary>
        /// Unique identifier of the <see cref="DbStats"/> used as primary key
        /// in the database.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Reference to the user profile associated with the stats.
        /// </summary>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Reference to the chapter associated with the stats.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Whether or not the chapter has been completed by the user.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Number of tips used for each page of the chapter. 
        /// </summary>
        public byte[]? Tips { get; set; }

        /// <summary>
        /// Number of submits performed for each page of the chapter.
        /// </summary>
        public byte[]? Submits { get; set; }

        /// <summary>
        /// Number of failed submits performed for each page of the chapter.
        /// </summary>
        public byte[]? Failures { get; set; }

        public static DbStats Create(Guid profileId, Guid chapterId, int pages)
        {
            return new DbStats {
                ProfileId = profileId,
                ChapterId = chapterId,
                Completed = false,
                Tips = new byte[pages],
                Submits = new byte[pages],
                Failures = new byte[pages]
            };
        }

        /// <summary>
        /// Updates the <see cref="Completed"/> property according to the current
        /// state of <see cref="Submits"/> and <see cref="Failures"/>.
        /// </summary>
        /// <returns>The same <see cref="DbStats"/> instance with updated <see cref="Completed"/>
        /// property value.</returns>
        public DbStats WithUpdatedCompleted()
        {
            bool PageCompleted(byte submits, int index)
            {
                var failures = Failures != null ? Failures[index] : 0;
                return submits > failures || submits == byte.MaxValue;
            }

            Completed = Submits != null && Submits.All(PageCompleted);
            return this;
        }

        public DbStats WithTip(int pageIndex)
        {
            Tips ??= new byte[pageIndex + 1];
            Tips.Increment(pageIndex);
            return this;
        }

        public DbStats WithSubmit(int pageIndex)
        {
            Submits ??= new byte[pageIndex + 1];
            Submits.Increment(pageIndex);
            return this;
        }

        public DbStats WithFailure(int pageIndex)
        {
            Failures ??= new byte[pageIndex + 1];
            Failures.Increment(pageIndex);
            return WithSubmit(pageIndex);
        }

        public void Validate()
        {
            if (Tips == null || Tips.Length == 0 || Submits == null || Submits.Length == 0 || Failures == null || Failures.Length == 0 || Tips.Length != Submits.Length || Submits.Length != Failures.Length)
            {
                throw new InvalidObjectException(this);
            }
        }
    }
}
