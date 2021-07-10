using Microsoft.Extensions.Configuration;

namespace Bhasha.Identity.Mongo.Configuration
{
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

            var dbName = section.GetValue<string>("Name");
            var hostname = section.GetValue<string>("Hostname");
            var username = section.GetValue<string>("User");
            var password = section.GetValue<string>("Password");

            return new MongoSettings
            {
                ConnectionString = Configuration.ConnectionString.ForMongoDB(hostname, username, password),
                DatabaseName = dbName
            };
        }
    }
}
