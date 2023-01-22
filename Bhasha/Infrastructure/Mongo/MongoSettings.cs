using Microsoft.Extensions.Configuration;

namespace Bhasha.Infrastructure.Mongo;

/// <summary>
/// Settings required to setup <see cref="MongoDB"/> for <see cref="Bhasha"/>.
/// </summary>
public class MongoSettings
{
    /// <summary>
    /// Connection string to a running MongoDB instance. 
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Name of the MongoDB database.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    public static MongoSettings From(IConfiguration config)
    {
        var section = config.GetSection("Database");

        var dbName = section.GetValue<string>("Name") ?? throw new ArgumentException("'Name' for MongoDB not set");
        var hostname = section.GetValue<string>("Hostname") ?? throw new ArgumentException("'Hostname' for MongoDB not set");
        var username = section.GetValue<string>("User") ?? throw new ArgumentException("'User' for MongoDB not set");
        var password = section.GetValue<string>("Password") ?? throw new ArgumentException("'Password' for MongoDB not set");

        return new MongoSettings
        {
            ConnectionString = Bhasha.ConnectionString.ForMongoDB(hostname, username, password, dbName),
            DatabaseName = dbName
        };
    }
}
