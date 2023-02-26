using System.Text;

namespace Bhasha;

public static class ConnectionString
{
    public static string ForMongoDB(string hostname, string username, string password, string database = "admin", string prefix = "mongodb", string? args = default)
    {
        var builder = new StringBuilder();

        builder.Append(prefix);
        builder.Append("://");
        builder.Append(username);
        builder.Append(':');
        builder.Append(password);
        builder.Append('@');
        builder.Append(hostname);
        builder.Append('/');
        builder.Append(database);

        if (string.IsNullOrWhiteSpace(args) is false)
        {
            builder.Append($"?{args}");
        }

        return builder.ToString();
    }
}
