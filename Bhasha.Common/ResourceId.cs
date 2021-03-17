namespace Bhasha.Common
{
    public class ResourceId : EntityId
    {
        public ResourceId(string id) : base(id)
        {
        }

        public static ResourceId? Create(string? id)
        {
            return id != default ? new ResourceId(id) : default;
        }

        public static implicit operator string?(ResourceId? resourceId)
        {
            return resourceId?.ToString();
        }

        public static implicit operator ResourceId?(string? resourceId)
        {
            return Create(resourceId);
        }
    }
}
