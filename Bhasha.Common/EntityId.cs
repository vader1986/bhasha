using System;

namespace Bhasha.Common
{
    public class EntityId : IEquatable<EntityId>
    {
        /// <summary>
        /// Unique identifier of the entity.
        /// </summary>
        public string Id { get; }

        public EntityId(string id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is EntityId other && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(EntityId other)
        {
            return Equals((object)other);
        }

        public static bool operator !=(EntityId? lhs, EntityId? rhs)
        {
            return !Equals(lhs, rhs);
        }

        public static bool operator ==(EntityId? lhs, EntityId? rhs)
        {
            return Equals(lhs, rhs);
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
