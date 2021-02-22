using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB.Collections
{
    public class Procedures : IQueryable<Procedure, ProcedureQuery>
    {
        private readonly IMongoDb _database;

        public Procedures(IMongoDb database)
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
            var result = await _database.Find<ProcedureDto>(Names.Collections.Procedures, x => x.ProcedureId == query.Id.Id, query.MaxItems);
            return result.Select(Converter.Convert);
        }

        private async ValueTask<IEnumerable<Procedure>> ExecuteQuery(ProcedureSupportQuery query)
        {
            var tokenType = query.SupportedType.ToString();
            var result = await _database.Find<ProcedureDto>(Names.Collections.Procedures,
                x => x.Support.Length == 0 ||
                     x.Support.Contains(tokenType),
                query.MaxItems);
            return result.Select(Converter.Convert);
        }
    }
}
