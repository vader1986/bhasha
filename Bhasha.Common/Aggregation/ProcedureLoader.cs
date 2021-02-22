using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Extensions;
using Bhasha.Common.Queries;

namespace Bhasha.Common.Aggregation
{
    public interface ILoadProcedures
    {
        ValueTask<Procedure[]> NextProcedures(IEnumerable<Translation> translations);
    }

    public class ProcedureLoader : ILoadProcedures
    {
        private readonly IQueryable<Procedure, ProcedureQuery> _procedures;

        public ProcedureLoader(IQueryable<Procedure, ProcedureQuery> procedures)
        {
            _procedures = procedures;
        }

        public async ValueTask<Procedure[]> NextProcedures(IEnumerable<Translation> translations)
        {
            var procedures = new List<Procedure>();
            var tokenTypes = translations.Select(t => t.Reference.TokenType).Distinct();

            foreach (var tokenType in tokenTypes)
            {
                var query = new ProcedureSupportQuery(1, tokenType);
                var result = await _procedures.Query(query);

                if (result.Any())
                {
                    procedures.AddRange(result);
                }
                else
                {
                    throw new NoProcedureFoundException($"could not find any procedure for {query.Stringify()}");
                }
            }

            return procedures.ToArray();
        }
    }
}
