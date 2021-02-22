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
    }
}
