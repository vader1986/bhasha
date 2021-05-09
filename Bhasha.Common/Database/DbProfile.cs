using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Model class for language profiles.
    /// </summary>
    public class DbProfile : ICanBeValidated, IEquatable<DbProfile?>
    {
        /// <summary>
        /// Native language of the user profile.
        /// </summary>
        public string? Native { get; set; }

        /// <summary>
        /// Target language to learn.
        /// </summary>
        public string? Target { get; set; }

        public void Validate()
        {
            if (Native == null || Target == null || Native == Language.Unknown || Target == Language.Unknown)
            {
                throw new InvalidObjectException(this);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbProfile);
        }

        public bool Equals(DbProfile? other)
        {
            return other != null &&
                   Native == other.Native &&
                   Target == other.Target;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Native, Target);
        }

        public static bool operator ==(DbProfile? left, DbProfile? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbProfile? left, DbProfile? right)
        {
            return !(left == right);
        }
    }
}
