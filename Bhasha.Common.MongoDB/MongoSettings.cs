namespace Bhasha.Common.MongoDB
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
    }
}
