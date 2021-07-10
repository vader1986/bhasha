namespace Bhasha.Identity.Mongo.Configuration
{
    public static class ConnectionString
    {
        public static string ForMongoDB(string hostname, string username, string password)
        {
            return $"mongodb://{username}:{password}@{hostname}";
        }
    }
}
