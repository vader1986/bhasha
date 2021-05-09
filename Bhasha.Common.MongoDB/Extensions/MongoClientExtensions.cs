using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Extensions
{
    public static class MongoClientExtensions
    {
        public static async Task<T> ExecuteAsync<T>(this MongoClient client, Func<MongoClient, Task<T>> action)
        {
            using var session = await client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var result = await action(client);
                await session.CommitTransactionAsync();
                return result;
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}
