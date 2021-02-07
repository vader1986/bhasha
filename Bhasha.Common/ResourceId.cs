#nullable enable
using System;
namespace Bhasha.Common
{
    public class ResourceId : IEquatable<ResourceId>
    {
        public string Id { get; }

        private ResourceId(string id)
        {
            Id = id;
        }

        public static ResourceId Create(string id)
        {
            return new ResourceId(id);
        }

        public bool Equals(ResourceId other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
