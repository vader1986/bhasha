namespace Bhasha.Common.MongoDB
{
    public static class ConnectionString
    {
        public static string Build(string hostname, string username, string password)
        {
            return $"mongodb://{username}:{password}@{hostname}";
        }
    }
}
