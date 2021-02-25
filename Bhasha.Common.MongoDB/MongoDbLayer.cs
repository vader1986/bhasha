using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB
{
    using Langs = Collections.Languages;

    public class MongoDbLayer : IDatabase
    {
        private readonly ProcedureIds _procedureIds;
        private readonly Procedures _procedures;
        private readonly Translations _translations;
        private readonly Langs _languages;

        public MongoDbLayer(string connectionString)
        {
            var db = MongoDb
                .Create(connectionString)
                .GetAwaiter()
                .GetResult();

            _procedureIds = new ProcedureIds(db);
            _procedures = new Procedures(db);
            _translations = new Translations(db);
            _languages = new Langs(db);
        }

        public ValueTask<IEnumerable<Translation>> Query(TranslationQuery query)
        {
            return _translations.Query(query);
        }

        public ValueTask<IEnumerable<Procedure>> Query(ProcedureQuery query)
        {
            return _procedures.Query(query);
        }

        ValueTask<IEnumerable<ProcedureId>> IListable<ProcedureId>.List()
        {
            return _procedureIds.List();
        }

        ValueTask<IEnumerable<Language>> IListable<Language>.List()
        {
            return _languages.List();
        }
    }
}
