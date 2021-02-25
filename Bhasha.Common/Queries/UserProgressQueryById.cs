namespace Bhasha.Common.Queries
{
    public class UserProgressQueryById : UserProgressQuery
    {
        public EntityId UserId { get; }

        public UserProgressQueryById(int maxItems, EntityId userId) : base(maxItems)
        {
            UserId = userId;
        }
    }
}
