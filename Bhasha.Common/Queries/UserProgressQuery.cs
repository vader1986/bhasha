namespace Bhasha.Common.Queries
{
    public abstract class UserProgressQuery
    {
        public int MaxItems { get; }

        public UserProgressQuery(int maxItems)
        {
            MaxItems = maxItems;
        }
    }
}
