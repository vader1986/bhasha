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
        private readonly IDatabase _database;

        public Procedures(IDatabase database)
        {
            _database = database;
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
            var result = await _database.Find<ProcedureDto>(Names.Collections.Procedures, x => x.ProcedureId == query.Id.Id);
            return result.Single().ToProcedure().ToEnumeration();
        }

        private async ValueTask<IEnumerable<Procedure>> ExecuteQuery(ProcedureSupportQuery query)
        {
            var tokenType = query.SupportedType.ToString();
            var result = await _database.Find<ProcedureDto>(Names.Collections.Procedures,
                x => x.Support.Length == 0 ||
                     x.Support.Contains(tokenType));
            return result.Select(x => x.ToProcedure());
        }
    }
}
