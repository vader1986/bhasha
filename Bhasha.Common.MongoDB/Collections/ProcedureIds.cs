using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB.Collections
{
    public class ProcedureIds : IListable<ProcedureId>
    {
        private readonly IMongoDb _database;

        public ProcedureIds(IMongoDb database)
        {
            _database = database;
        }

        public async ValueTask<IEnumerable<ProcedureId>> List()
        {
            var procedureIds = await _database.List<ProcedureDto, string>(
                Names.Collections.Procedures,
                x => x.ProcedureId);

            return procedureIds.Select(x => new ProcedureId(x));
        }
    }
}
