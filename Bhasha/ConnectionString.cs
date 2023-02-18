namespace Bhasha;

public static class ConnectionString
{
    public static string ForMongoDB(string hostname, string username, string password, string database = "admin")
    {
        return $"mongodb://{username}:{password}@{hostname}/{database}";
    }
}
