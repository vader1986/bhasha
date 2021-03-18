namespace Bhasha.Common
{
    public class ResourceId : EntityId
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

        public override string ToString()
        {
            return Id;
        }
    }
}
