using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Collections
{
    public class Procedures : IQueryable<Procedure, ProcedureQuery>
    {
        private readonly Database _database;

        public Procedures(Database database)
        {
            _database = database;
        }

        public Task<IEnumerable<Procedure>> Query(ProcedureQuery query)
        {
            switch (query)
            {
                case ProcedureIdQuery iq:
                    return ExecuteQuery(iq);

                case ProcedureSupportQuery sq:
                    return ExecuteQuery(sq);

                default:
                    throw new ArgumentOutOfRangeException(nameof(query),
                        $"Query type {query.GetType().FullName} not supported");
            }
        }

        private async Task<IEnumerable<Procedure>> ExecuteQuery(ProcedureIdQuery query)
        {
            var collection = _database.GetCollection<ProcedureDto>(Names.Collections.Procedures);
            var result = await collection.FindAsync(x => x.ProcedureId == query.Id.Id);

            return result.Single().ToProcedure().ToEnumeration();
        }

        private async Task<IEnumerable<Procedure>> ExecuteQuery(ProcedureSupportQuery query)
        {
            var collection = _database.GetCollection<ProcedureDto>(Names.Collections.Procedures);
            var tokenType = query.SupportedType.ToString();
            var result = await collection.FindAsync(x => x.Support.Length == 0 || x.Support.Contains(tokenType));

            throw new Exception(); // TODO
        }
    }
}
