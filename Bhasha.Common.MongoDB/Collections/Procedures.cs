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
    public class Procedures : IQueryable<Procedure, ProcedureQuery>, IListable<ProcedureId>
    {
        private readonly Database _database;

        public Procedures(Database database)
        {
            _database = database;
        }

        public async ValueTask<IEnumerable<ProcedureId>> List()
        {
            var collection = _database.GetCollection<ProcedureDto>(Names.Collections.Procedures);
            var procedureIds = await collection.DistinctAsync(x => x.ProcedureId, x => true);

            return procedureIds.ToEnumerable().Select(x => new ProcedureId(x));
        }

        public ValueTask<IEnumerable<Procedure>> Query(ProcedureQuery query)
        {
            return query switch
            {
                ProcedureIdQuery iq => ExecuteQuery(iq),
                ProcedureSupportQuery sq => ExecuteQuery(sq),
                _ => OnDefault(query)
            };
        }

        private static ValueTask<IEnumerable<Procedure>> OnDefault(ProcedureQuery query)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query),
                $"Query type {query.GetType().FullName} not supported");
        }

        private async ValueTask<IEnumerable<Procedure>> ExecuteQuery(ProcedureIdQuery query)
        {
            var collection = _database.GetCollection<ProcedureDto>(Names.Collections.Procedures);
            var result = await collection.FindAsync(x => x.ProcedureId == query.Id.Id);

            return result.Single().ToProcedure().ToEnumeration();
        }

        private async ValueTask<IEnumerable<Procedure>> ExecuteQuery(ProcedureSupportQuery query)
        {
            var collection = _database.GetCollection<ProcedureDto>(Names.Collections.Procedures);
            var tokenType = query.SupportedType.ToString();
            var result = await collection.FindAsync(x => x.Support.Length == 0 || x.Support.Contains(tokenType));

            return result.ToEnumerable().Select(x => x.ToProcedure());
        }
    }
}
