using System;

namespace Bhasha.Common
{
    public class ResourceId : EntityId, IEquatable<ResourceId?>
    {
        public ResourceId(string id) : base(id)
        {

        }

        public static implicit operator string?(ResourceId? resourceId)
        {
            return resourceId?.ToString();
        }

        public static implicit operator ResourceId?(string? resourceId)
        {
            return resourceId != default ? new ResourceId(resourceId) : default;
        }

        public static bool operator ==(ResourceId? left, ResourceId? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ResourceId? left, ResourceId? right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ResourceId);
        }

        public bool Equals(ResourceId? other)
        {
            return other != null && base.Equals(other) && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id);
        }
    }
}
